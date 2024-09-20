using battleship_royale_be.Data;
using battleship_royale_be.Models;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Usecase.Shoot
{
    public class ShootUseCase : IShootUseCase
    {
        private readonly BattleshipAPIContext _context;

        public ShootUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public async Task<Game?> Shoot(Guid id, ShotCoordinates shotCoords)
        {
            Game? gameToUpdate = await _context.Games
                .Include(game => game.Cells)
                .Include(game => game.Ships)
                  .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (gameToUpdate == null)
                return null;

            Game gameAfterShot = ShotHandler.HandleShot(gameToUpdate, shotCoords);

            foreach (Cell cell in gameToUpdate.Cells)
                _context.Cells.Remove(cell);

            foreach (Ship ship in gameToUpdate.Ships)
            {
                foreach (Coordinates coords in ship.Coordinates)
                {
                    _context.Coordinates.Remove(coords);
                }
                _context.Ships.Remove(ship);
            }

            _context.Games.Remove(gameToUpdate);

            _context.Games.Add(gameAfterShot);
            await _context.SaveChangesAsync();
            return gameAfterShot;
        }
    }
}
