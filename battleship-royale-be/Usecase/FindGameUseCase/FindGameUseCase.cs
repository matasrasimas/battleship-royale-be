
using battleship_royale_be.Data;
using battleship_royale_be.Models.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.Security.Provider;

namespace battleship_royale_be.Usecase.FindGameUseCase
{
    public class FindGameUseCase : IFindGameUseCase
    {
        private readonly BattleshipAPIContext _context;

        public FindGameUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public async Task<Guid> FindGame()
        {
            var availableGame = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Players.Count < 2)
                .FirstOrDefaultAsync();

            if (availableGame == null) {
                var newGame = GameBuilder.DefaultValues().Build();
                await _context.Games.AddAsync(newGame);
                await _context.SaveChangesAsync();
                return newGame.Id;
            }

            await _context.SaveChangesAsync();
            return availableGame.Id;
        }
    }
}
