using System.Text.RegularExpressions;
using battleship_royale_be.Data;
using battleship_royale_be.DesignPatterns.Facade;
using battleship_royale_be.DesignPatterns.Interpreter;
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
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Hubs
{
    public partial class GameHub : Hub
    {
        private GameFacade _gameFacade;

        public GameHub(BattleshipAPIContext context,
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
            _gameFacade = new GameFacade(context,
                createNewPlayerUseCase,
                getGameByIdUseCase,
                shootUseCase,
                addPlayerToGameUseCase,
                surrenderUseCase,
                pauseUseCase,
                findGameUseCase,
                commandController,
                server);
        }



        public async Task MoveShipsByHitPoints(string playerId, int hitPoints)
        {

            // Log the start of the method call
            Console.WriteLine($"MoveShipsByHitPoints called for player {playerId} with hitPoints: {hitPoints}");
            Player player = await _gameFacade.GetPlayerById(playerId);

            if (player == null)
            {
                // Log when player is not found
                Console.WriteLine($"Player with ID {playerId} not found.");
                await Clients.Caller.SendAsync("LogMessage", $"Player with ID {playerId} not found.");
                return;
            }

            if (player.Ships != null && player.Ships.Count > 0)
            {
                // Log the player and ship count
                Console.WriteLine($"Player {playerId} has {player.Ships.Count} ships.");

                foreach (var ship in player.Ships)
                {
                    // Log the ship's initial state before moving
                    string beforeMoveMessage = $"Before moving: Ship {ship.HitPoints} is at position {ship.Coordinates}.";
                    Console.WriteLine(beforeMoveMessage);  // Log to console
                    await Clients.Caller.SendAsync("LogMessage", beforeMoveMessage);

                    // Move the ship by the given hit points
                    ship.MoveByHitPoints(hitPoints);

                    // Log the ship's state after moving
                    string afterMoveMessage = $"After moving: Ship {ship.HitPoints} is now at position {ship.Coordinates}.";
                    Console.WriteLine(afterMoveMessage);  // Log to console
                    await Clients.Caller.SendAsync("LogMessage", afterMoveMessage);
                }

                // Notify all clients that ships have moved
                Console.WriteLine($"Ships moved for player {playerId} by {hitPoints} hit points.");
                await Clients.All.SendAsync("ShipsMoved", hitPoints);
            }
            else
            {
                // Log when no ships are found for the player
                Console.WriteLine($"No ships found for player {playerId}.");
                await Clients.Caller.SendAsync("LogMessage", "No ships found for player.");
            }
        }








        public async Task UpdateGameTime(int timeRemaining)
        {
            // Update the singleton instance
            GameTime.Instance.UpdateTime(timeRemaining);

            // Notify all clients about the updated time
            await Clients.All.SendAsync("ReceiveTimeUpdate", GameTime.Instance.TimeRemaining);
        }


        public async Task JoinSpecificGame(UserConnection conn)
        {
            var gameToJoin = await _gameFacade.FindGameById(conn.GameId);

            if (gameToJoin.Players.Count >= 2)
            {
                await Clients.Caller
                    .SendAsync("JoinSpecificGameError", conn.Id, "game is full");
                return;
            }

            var gameWithAddedPlayer = await _gameFacade.AddPlayerToGame(gameToJoin, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);

            await Clients.Group(conn.GameId)
                .SendAsync("JoinSpecificGame", "admin", gameWithAddedPlayer);
        }

        public async Task MakeShot(ShotCoordinates shotCoords, int shotCount)
        {
            var conn = await _gameFacade.GetUserConnectionById(Context.ConnectionId);
            if (conn != null)
            {
                Game gameAfterShot = await _gameFacade.MakeShot(shotCoords, shotCount, conn);
                if (gameAfterShot != null)
                {
                    _gameFacade.NotifyAll("Player " + Context.ConnectionId + " made a shot at " + (shotCoords.Row + 1) + " " + (shotCoords.Col + 1));
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveGameAfterShot", conn.Id, gameAfterShot);
                }
            }
        }

        public async Task HandleSurrender()
        {
            Game gameAfterSurrender = await _gameFacade.TryToSurrender(Context.ConnectionId);
            if (gameAfterSurrender != null)
                await Clients.Group(gameAfterSurrender.Id.ToString())
                    .SendAsync("ReceiveGameAfterSurrender", Context.ConnectionId, gameAfterSurrender);
        }

        public async Task GoToNextLevel(UserConnection conn)
        {
            var nextLevelGame = await _gameFacade.GetNextLevel(conn);

            if (nextLevelGame != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);

                await Clients.Group(conn.GameId)
                    .SendAsync("ReceiveGameAfterGoToNextLevel", "admin", nextLevelGame);
            }
        }

        public async Task SendMessage(string message)
        {
            IExpression expression = CommandParser.Parse(message);
            await expression.Interpret(this);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveSpecificGame();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveSpecificGame()
        {
            var removedConnection = await _gameFacade.RemoveConnectionById(Context.ConnectionId);
            if (removedConnection != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, removedConnection.GameId);
            }
        }

        public async Task GetConnectionId()
        {
            await Clients.Caller
                .SendAsync("GetYourConnectionId", Context.ConnectionId, Context.ConnectionId);
        }
        public async Task MessageCommand(string message)
        {
            var conn = await _gameFacade.GetUserConnectionById(Context.ConnectionId);
            if (conn != null)
            {
                var player = await _gameFacade.GetPlayerById(conn.Id);
                if (player != null)
                {
                    var playerIndex = await _gameFacade.GetPlayerIndex(player);
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveMessage", "Player " + playerIndex, message);
                }
            }
        }
        public async Task SurrenderCommand()
        {
            await HandleSurrender();
        }
        public async Task ShootCommand(ShotCoordinates shotCoords)
        {
            var conn = await _gameFacade.GetUserConnectionById(Context.ConnectionId);
            if (conn != null)
            {
                var player = await _gameFacade.GetPlayerById(conn.Id);
                if (player != null)
                {
                    if (!player.IsYourTurn)
                    {
                        await Clients.Caller
                            .SendAsync("ReceiveMessage", "System", "Cannot shoot: It's not your turn");
                        return;
                    }
                    else
                    {
                        await MakeShot(shotCoords, 1);
                    }
                }
            }
        }
        public async Task PauseCommand()
        {
            var conn = await _gameFacade.GetUserConnectionById(Context.ConnectionId);
            Game gameAfterPause = await _gameFacade.PauseGame(conn);
            if (gameAfterPause != null)
            {
                await Clients.Group(conn.GameId)
                    .SendAsync("ReceiveGameAfterCommand", gameAfterPause);
            }
        }
        public async Task UndoCommand()
        {
            var conn = await _gameFacade.GetUserConnectionById(Context.ConnectionId);
            Game backup = await _gameFacade.Undo(conn.Id);

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
        }
        public async Task InvalidCommand()
        {
            await Clients.Caller
                .SendAsync("ReceiveMessage", "System", "Command not found");
        }
    }
}
