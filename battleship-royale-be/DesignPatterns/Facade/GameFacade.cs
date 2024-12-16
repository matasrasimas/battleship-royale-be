using battleship_royale_be.Data;
using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using battleship_royale_be.DesignPatterns.Memento;
using battleship_royale_be.DesignPatterns.Visitor;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Command;
using battleship_royale_be.Models.Observer;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.FindGameUseCase;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Move;
using battleship_royale_be.Usecase.Pause;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.Surrender;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace battleship_royale_be.DesignPatterns.Facade
{
    public class GameFacade
    {
        private readonly BattleshipAPIContext _context;
        private readonly ICreateNewPlayerUseCase _createNewPlayerUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;
        private readonly IMoveUseCase _moveUseCase;
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
            IMoveUseCase moveUseCase,
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
            _moveUseCase = moveUseCase;
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

        public async Task<Caretaker> FindPlayerSnapshot(string PlayerGuid)
        {
            return await _context.Caretakers.Where(care => care.PlayerId == PlayerGuid).FirstOrDefaultAsync();
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

        public async Task CreateSnapshot(UserConnection user)
        {
            Game? gameToSave = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id.ToString() == user.GameId)
                .FirstOrDefaultAsync();

            Caretaker caretaker = new Caretaker(user.Id, gameToSave);
            caretaker.Backup();
            await _context.Caretakers.AddAsync(caretaker);
            await _context.SaveChangesAsync();
        }

        public async Task<Game> UseSnapshot(Caretaker caretaker, UserConnection conn, Game gameBackup)
        {
            caretaker.Undo();
            Game gameToLoad = await FindGameById(conn.GameId);
            Game game = GameBuilder.From(gameToLoad).SetPlayers(gameBackup.Players).SetShotResultMessage(gameBackup.ShotResultMessage).Build();
            foreach (Player player in gameToLoad.Players)
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
            _context.Games.Remove(gameToLoad);
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
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
                randomPlayer.ShotsRemaining = 1;
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

        public async Task<Game> MoveShipsByHitPoints(int hitpoints, UserConnection conn)
        {
            return await _commandController.Run(new MoveCommand(_moveUseCase, Guid.Parse(conn.GameId), conn.Id, hitpoints));
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

        public List<IShip> ConvertToShipArray(List<Cell> cells)
        {
            List<IShip> array = new List<IShip>();
            foreach (Cell cell in cells)
            {
                if (cell.IsShip)
                {
                    if (cell.ImagePath == "/images/battleship.png")
                    {
                        array.Add(new Battleship());
                    }
                    if (cell.ImagePath == "/images/carrier.png")
                    {
                        array.Add(new Carrier());
                    }
                    if (cell.ImagePath == "/images/cruiser.png")
                    {
                        array.Add(new Cruiser());
                    }
                    if (cell.ImagePath == "/images/destroyer.png")
                    {
                        array.Add(new Destroyer());
                    }
                    if (cell.ImagePath == "/images/submarine.png")
                    {
                        array.Add(new Submarine());
                    }
                }
                else
                {
                    array.Add(null);
                }
            }
            return array;
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

        public async Task<Game> ApplySkin(UserConnection conn, IVisitor visitor)
        {
            Game game = await FindGameById(conn.GameId);

            foreach (Player p in game.Players)
            {
                foreach (Cell cell in p.Cells)
                {
                    _context.Cells.Remove(cell);
                }
                foreach (Ship ship in p.Ships)
                {
                    foreach (Coordinates coord in ship.Coordinates)
                    {
                        _context.Coordinates.Remove(coord);
                    }
                    _context.Ships.Remove(ship);
                }
                _context.Players.Remove(p);
            }
            _context.Games.Remove(game);

            Game gameToSave = GameBuilder.From(game).SetPlayers(game.Players).SetShotResultMessage(game.ShotResultMessage).Build();
            

            Player player = gameToSave.Players.FirstOrDefault(p => p.ConnectionId == conn.Id);
            List<IShip> list = ConvertToShipArray(player.Cells);

            foreach (var ship in list)
            {
                if (ship != null)
                {
                    ship.Accept(visitor);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    player.Cells[i].Color = list[i].GetColor();
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(gameToSave, Formatting.Indented));
            await _context.Games.AddAsync(gameToSave);
            await _context.SaveChangesAsync();
            return gameToSave;
        }


    }
}
