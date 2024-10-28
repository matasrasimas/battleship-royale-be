using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.Surrender;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Hubs
{
    public class GameHub : Hub
    {
        private readonly BattleshipAPIContext _context;
        private readonly ICreateNewPlayerUseCase _createNewPlayerUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;
        private readonly IAddPlayerToGameUseCase _addPlayerToGameUseCase;
        private readonly ISurrenderUseCase _surrenderUseCase;

        public GameHub(BattleshipAPIContext context,
            ICreateNewPlayerUseCase createNewPlayerUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase,
            IAddPlayerToGameUseCase addPlayerToGameUseCase,
            ISurrenderUseCase surrenderUseCase)
        {
            _context = context;
            _createNewPlayerUseCase = createNewPlayerUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _shootUseCase = shootUseCase;
            _addPlayerToGameUseCase = addPlayerToGameUseCase;
            _surrenderUseCase = surrenderUseCase;
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

            var playerToAdd = _createNewPlayerUseCase.CreatePlayer(Context.ConnectionId);

            Game gameAfterAddedPlayer = GameBuilder.From(gameToJoin).Build();
            gameAfterAddedPlayer.Players.Add(playerToAdd);

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
                Game gameAfterShot = await _shootUseCase.Shoot(Guid.Parse(conn.GameId), shotCoords, conn.Id, shotCount);
                if (gameAfterShot != null)
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveGameAfterShot", conn.Id, gameAfterShot);

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
    }
}
