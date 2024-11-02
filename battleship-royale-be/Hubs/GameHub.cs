using System.Text.RegularExpressions;
using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Command;
using battleship_royale_be.Models.Observer;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Pause;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.Surrender;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Hubs
{
    public partial class GameHub : Hub
    {
        private readonly BattleshipAPIContext _context;
        private readonly ICreateNewPlayerUseCase _createNewPlayerUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;
        private readonly IAddPlayerToGameUseCase _addPlayerToGameUseCase;
        private readonly ISurrenderUseCase _surrenderUseCase;
        private readonly IPauseUseCase _pauseUseCase;

        private CommandController _commandController;
        private Subject _server;

        public GameHub(BattleshipAPIContext context,
            ICreateNewPlayerUseCase createNewPlayerUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase,
            IAddPlayerToGameUseCase addPlayerToGameUseCase,
            ISurrenderUseCase surrenderUseCase,
            IPauseUseCase pauseUseCase,
            CommandController commandController,
            Subject server)
        {
            _context = context;
            _createNewPlayerUseCase = createNewPlayerUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _shootUseCase = shootUseCase;
            _addPlayerToGameUseCase = addPlayerToGameUseCase;
            _surrenderUseCase = surrenderUseCase;
            _pauseUseCase = pauseUseCase;
            _commandController = commandController;
            _server = server;
        }

        public async Task JoinSpecificGame(UserConnection conn)
        {
            var gameToJoin = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id.ToString() == conn.GameId)
                .FirstOrDefaultAsync();

            if (gameToJoin.Players.Count >= 2)
            {
                await Clients.Caller
                    .SendAsync("JoinSpecificGameError", conn.Id, "game is full");
                return;
            }

            var playerToAdd = _createNewPlayerUseCase.CreatePlayer(Context.ConnectionId, 1);

            Game gameAfterAddedPlayer = GameBuilder.From(gameToJoin).Build();
            gameAfterAddedPlayer.Players.Add(playerToAdd);
            _server.Attach(playerToAdd);
            _server.NotifyAll("Player " + Context.ConnectionId + " joined the game");

            if (gameAfterAddedPlayer.Players.Count >= 2)
            {
                Random random = new Random();
                int randomIndex = random.Next(gameAfterAddedPlayer.Players.Count);
                Player randomPlayer = gameAfterAddedPlayer.Players[randomIndex];
                randomPlayer.IsYourTurn = true;
            }

            foreach (Player player in gameToJoin.Players)
            {
                foreach (Cell cell in player.Cells)
                {
                    _context.Cells.Remove(cell);
                }
                foreach (Ship ship in player.Ships)
                {
                    foreach (Coordinates coord in ship.Coordinates)
                    {
                        _context.Coordinates.Remove(coord);
                    }
                    _context.Ships.Remove(ship);
                }
                _context.Players.Remove(player);
            }
            _context.Games.Remove(gameToJoin);

            await _context.Games.AddAsync(gameAfterAddedPlayer);
            await _context.UserConnections.AddAsync(new UserConnection(Context.ConnectionId, conn.GameId));

            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);

            await Clients.Group(conn.GameId)
                .SendAsync("JoinSpecificGame", "admin", gameAfterAddedPlayer);
        }

        public async Task MakeShot(ShotCoordinates shotCoords, int shotCount)
        {
            var conn = await _context.UserConnections.Where(conn => conn.Id == Context.ConnectionId).FirstOrDefaultAsync();
            if (conn != null)
            {
                //Game gameAfterShot = await _shootUseCase.Shoot(Guid.Parse(conn.GameId), shotCoords, conn.Id, shotCount);
                Game gameAfterShot = await _commandController.Run(new ShootCommand(_shootUseCase, Guid.Parse(conn.GameId), shotCoords, conn.Id, shotCount));
                if (gameAfterShot != null)
                {
                    _server.NotifyAll("Player " + Context.ConnectionId + " made a shot at " + (shotCoords.Row + 1) + " " + (shotCoords.Col + 1));
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveGameAfterShot", conn.Id, gameAfterShot);
                }
            }
        }

        public async Task HandleSurrender()
        {
            var conn = await _context.UserConnections.Where(conn => conn.Id == Context.ConnectionId).FirstOrDefaultAsync();
            if (conn != null)
            {
                Game gameAfterSurrender = await _surrenderUseCase.Surrender(Guid.Parse(conn.GameId), conn.Id);
                if (gameAfterSurrender != null)
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveGameAfterSurrender", conn.Id, gameAfterSurrender);
            }
        }

        public async Task GoToNextLevel(UserConnection conn)
        {
            var gameToUpdate = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id.ToString().ToLower() == conn.GameId)
                .FirstOrDefaultAsync();

            if (gameToUpdate != null)
            {
                List<Player> nextLevelPlayers = new List<Player>();
                foreach (Player player in gameToUpdate.Players)
                {
                    nextLevelPlayers.Add(_createNewPlayerUseCase.CreatePlayer(player.ConnectionId, 2));
                }

                Game gameWithUpdatedPlayers = GameBuilder.From(gameToUpdate).SetPlayers(nextLevelPlayers).Build();
                Random random = new Random();
                int randomIndex = random.Next(gameWithUpdatedPlayers.Players.Count);
                Player randomPlayer = gameWithUpdatedPlayers.Players[randomIndex];
                randomPlayer.IsYourTurn = true;

                foreach (Player player in gameToUpdate.Players)
                {
                    foreach (Cell cell in player.Cells)
                    {
                        _context.Cells.Remove(cell);
                    }
                    foreach (Ship ship in player.Ships)
                    {
                        foreach (Coordinates coord in ship.Coordinates)
                        {
                            _context.Coordinates.Remove(coord);
                        }
                        _context.Ships.Remove(ship);
                    }
                    _context.Players.Remove(player);
                }
                _context.Games.Remove(gameToUpdate);

                await _context.Games.AddAsync(gameWithUpdatedPlayers);

                await _context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);

                await Clients.Group(conn.GameId)
                    .SendAsync("ReceiveGameAfterGoToNextLevel", "admin", gameWithUpdatedPlayers);
            }
        }

        public async Task SendMessage(string message)
        {
            var conn = await _context.UserConnections.Where(conn => conn.Id == Context.ConnectionId).FirstOrDefaultAsync();
            if (conn != null)
            {
                var player = _context.Players.Where(player => player.ConnectionId == conn.Id).FirstOrDefault();
                if (player != null)
                {
                    var playerIndex = _context.Players.ToList().IndexOf(player);
                    if (message.StartsWith('/'))
                    {
                        switch (message)
                        {
                            case "/surrender":
                                await HandleSurrender();
                                break;
                            case var str when MyRegex().IsMatch(str):
                                var shotCoords = new ShotCoordinates(
                                    int.Parse(message.Split(' ')[1]) - 1,
                                    int.Parse(message.Split(' ')[2]) - 1
                                );
                                if (!player.IsYourTurn)
                                {
                                    await Clients.Caller
                                        .SendAsync("ReceiveMessage", "System", "Cannot shoot: It's not your turn");
                                    return;
                                }
                                else
                                {
                                    // TODO : add check for valid coordinates
                                    await MakeShot(shotCoords, 1);
                                    //await Clients.Caller
                                    //    .SendAsync("ReceiveMessage", "System", "You made a shot at " + (shotCoords.Row + 1) + " " + (shotCoords.Col + 1));
                                }
                                break;
                            case "/pause":
                                Game gameAfterPause = await _commandController.Run(new PauseCommand(_pauseUseCase, Guid.Parse(conn.GameId), conn.Id));
                                if (gameAfterPause != null)
                                {
                                    await Clients.Group(conn.GameId)
                                        .SendAsync("ReceiveGameAfterCommand", gameAfterPause);
                                }
                                break;
                            case "/undo":
                                Game backup = await _commandController.Undo(_context, conn.Id);
                                                            
                                if (backup == null)
                                {
                                    await Clients.Caller
                                        .SendAsync("ReceiveMessage", "System", "Cannot undo");                                    
                                }
                                else
                                {
                                    await Clients.Group(conn.GameId)
                                        .SendAsync("ReceiveGameAfterCommand", backup);
                                }
                                break;
                            default:
                                await Clients.Caller
                                    .SendAsync("ReceiveMessage", "System", "Command not found");
                                break;
                        }
                    }
                    else
                    {
                        await Clients.Group(conn.GameId)
                            .SendAsync("ReceiveMessage", "Player " + playerIndex, message);
                    }
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveSpecificGame();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveSpecificGame()
        {
            var connectionToRemove = await _context.UserConnections.Where(conn => conn.Id == Context.ConnectionId).FirstOrDefaultAsync();
            if (connectionToRemove != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionToRemove.GameId);
                _context.UserConnections.Remove(connectionToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task GetConnectionId()
        {
            await Clients.Caller
                .SendAsync("GetYourConnectionId", Context.ConnectionId, Context.ConnectionId);
        }
        [GeneratedRegex(@"^/shoot \d+ \d+$")]
        private static partial Regex MyRegex();
    }
}
