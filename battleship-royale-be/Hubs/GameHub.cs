using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.StartNewGame;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Hubs
{
    public class GameHub : Hub
    {
        private readonly BattleshipAPIContext _context;
        private readonly IStartNewGameUseCase _startNewGameUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;

        public GameHub(BattleshipAPIContext context,
            IStartNewGameUseCase startNewGameUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase)
        {
            _context = context;
            _startNewGameUseCase = startNewGameUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _shootUseCase = shootUseCase;
        }

        public async Task JoinSpecificGame(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.GameId);

            await _context.UserConnections.AddAsync(new UserConnection(Context.ConnectionId, conn.GameId));
            await _context.SaveChangesAsync();

            await Clients.Group(conn.GameId)
                .SendAsync("JoinSpecificGame", "admin", $"User has joined the game {conn.GameId}");
        }

        public async Task MakeShot(ShotCoordinates shotCoords) {
            var conn = await _context.UserConnections.Where(conn => conn.Id == Context.ConnectionId).FirstOrDefaultAsync();
            if (conn != null) {
                Game gameAfterShot = await _shootUseCase.Shoot(Guid.Parse(conn.GameId), shotCoords);
                if(gameAfterShot != null)
                    await Clients.Group(conn.GameId)
                        .SendAsync("ReceiveGameAfterShot", conn.Id, gameAfterShot);

            }
        }
    }
}
