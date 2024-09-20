using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Usecase.GetGameById
{
    public class GetGameByIdUseCase : IGetGameByIdUseCase
    {
        private readonly BattleshipAPIContext _context;

        public GetGameByIdUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public async Task<Game?> Get(Guid id)
        {
            var games = await _context.Games
                .Include(game => game.Cells)
                .Include(game => game.Ships)
                  .ThenInclude(ship => ship.Coordinates)
                .ToListAsync();

            if (games == null)
                return null;

            var game = await _context.Games
                .Include(game => game.Cells)
                .Include(game => game.Ships)
                  .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if(game == null)
                return null;

            List<Cell> sortedCells = game.Cells
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();

            return GameBuilder.From(game).SetCells(sortedCells).Build();
        }
    }
}
