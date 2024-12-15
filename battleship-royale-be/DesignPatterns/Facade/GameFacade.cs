using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Command;
using battleship_royale_be.Models.Observer;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.FindGameUseCase;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Pause;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.Surrender;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace battleship_royale_be.DesignPatterns.Facade
{
    public class GameFacade
    {
        private readonly BattleshipAPIContext _context;
        private readonly ICreateNewPlayerUseCase _createNewPlayerUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;
        private readonly IAddPlayerToGameUseCase _addPlayerToGameUseCase;
        private readonly ISurrenderUseCase _surrenderUseCase;
        private readonly IPauseUseCase _pauseUseCase;
        private readonly IFindGameUseCase _findGameUseCase;
        private CommandController _commandController;
        private Subject _server;

        public GameFacade(BattleshipAPIContext context,
            ICreateNewPlayerUseCase createNewPlayerUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase,
            IAddPlayerToGameUseCase addPlayerToGameUseCase,
            ISurrenderUseCase surrenderUseCase,
            IPauseUseCase pauseUseCase,
            IFindGameUseCase findGameUseCase,
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
            _findGameUseCase = findGameUseCase;
            _commandController = commandController;
            _server = server;
        }

        public async Task<Game> FindGameById(string gameId)
        {
            var gameToJoin = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id.ToString() == gameId)
                .FirstOrDefaultAsync();
            return gameToJoin;
        }

        public async Task<Game> FindGameById(Guid gameId)
        {
            return await _getGameByIdUseCase.Get(gameId);
        }

        public async Task<Guid> FindAvailableGameId()
        {
            return await _findGameUseCase.FindGame();
        }

        public async Task<UserConnection> GetUserConnectionById(string connectionId)
        {
            return await _context.UserConnections.Where(conn => conn.Id == connectionId).FirstOrDefaultAsync();
        }

        public async Task<Player> GetPlayerById(string connectionId)
        {
            Console.WriteLine($"Searching for player with ConnectionId: {connectionId}");

            var player = await _context.Players
                                        .Include(p => p.Ships)  // Eagerly load Ships
                                        .Where(player => player.ConnectionId == connectionId)
                                        .FirstOrDefaultAsync();

            if (player != null)
            {
                Console.WriteLine($"Player found: {player.Id} with ConnectionId: {player.ConnectionId}");
                Console.WriteLine($"Player {player.Id} has {player.Ships?.Count ?? 0} ships.");
            }
            else
            {
                Console.WriteLine($"No player found with ConnectionId: {connectionId}");
            }

            return player;
        }

        public async Task<int> GetPlayerIndex(Player player)
        {
            var playerList = await _context.Players.ToListAsync();
            return playerList.IndexOf(player);
        }

        public async Task<Game> PauseGame(UserConnection conn)
        {
            return await _commandController.Run(new PauseCommand(_pauseUseCase, Guid.Parse(conn.GameId), conn.Id));
        }

        public async Task<Game> Undo(string connectionId)
        {
            return await _commandController.Undo(_context, connectionId);
        }

        public void NotifyAll(string msg)
        {
            _server.NotifyAll(msg);
        }

        public async Task<Game> AddPlayerToGame(Game game, string connectionId)
        {
            var playerToAdd = _createNewPlayerUseCase.CreatePlayer(connectionId, 1);

            Game gameAfterAddedPlayer = GameBuilder.From(game).Build();
            gameAfterAddedPlayer.Players.Add(playerToAdd);
            _server.Attach(playerToAdd);
            NotifyAll("Player " + connectionId + " joined the game");

            if (gameAfterAddedPlayer.Players.Count >= 2)
            {
                Random random = new Random();
                int randomIndex = random.Next(gameAfterAddedPlayer.Players.Count);
                Player randomPlayer = gameAfterAddedPlayer.Players[randomIndex];
                randomPlayer.IsYourTurn = true;
            }

            foreach (Player player in game.Players)
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
            _context.Games.Remove(game);

            await _context.Games.AddAsync(gameAfterAddedPlayer);
            await _context.UserConnections.AddAsync(new UserConnection(connectionId, gameAfterAddedPlayer.Id.ToString()));

            await _context.SaveChangesAsync();

            return gameAfterAddedPlayer;
        }

        public async Task<Game> MakeShot(ShotCoordinates shotCoords, int shotCount, UserConnection conn)
        {
            return await _commandController.Run(new ShootCommand(_shootUseCase, Guid.Parse(conn.GameId), shotCoords, conn.Id, shotCount));
        }

        public async Task<Game> TryToSurrender(string connectionId)
        {
            var conn = await GetUserConnectionById(connectionId);
            if (conn == null)
            {
                return null;
            }

            return await _surrenderUseCase.Surrender(Guid.Parse(conn.GameId), conn.Id);
        }

        public async Task<UserConnection> RemoveConnectionById(string connectionId)
        {
            var connectionToRemove = await _context.UserConnections.Where(conn => conn.Id == connectionId).FirstOrDefaultAsync();
            if (connectionToRemove != null)
            {
                _context.UserConnections.Remove(connectionToRemove);
                await _context.SaveChangesAsync();
            }
            return connectionToRemove;
        }

        public async Task<Game> GetNextLevel(UserConnection conn)
        {
            var gameToUpdate = await FindGameById(conn.GameId);

            if (gameToUpdate == null)
            {
                return null;
            }

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
            return gameWithUpdatedPlayers;
        }
    }
}
