using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
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

        public async Task<Game?> Shoot(Guid id, ShotCoordinates shotCoords, string connectionId)
        {
            Game? gameToUpdate = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (gameToUpdate == null)
                return null;

            Player? targetPlayer = gameToUpdate.Players.Where(player => player.ConnectionId != connectionId).FirstOrDefault();
            if (targetPlayer == null)
                return null;

            Player? attackerPlayer = gameToUpdate.Players.Where(player => player.ConnectionId == connectionId).FirstOrDefault();
            if (attackerPlayer == null)
                return null;

            List<Player> playersAfterShot = ShotHandler.HandleShot(attackerPlayer, targetPlayer, shotCoords);
            List<Player> updatedPlayersList = new List<Player>();
            foreach (Player player in playersAfterShot) {
                updatedPlayersList.Add(PlayerBuilder.From(player).Build());
            }

            Game gameAfterShot = GameBuilder.From(gameToUpdate).SetPlayers(updatedPlayersList).Build();

            foreach (Player player in gameToUpdate.Players) {
                foreach (Cell cell in player.Cells)
                    _context.Cells.Remove(cell);

                foreach (Ship ship in player.Ships)
                {
                    foreach (Coordinates coords in ship.Coordinates)
                    {
                        _context.Coordinates.Remove(coords);
                    }
                    _context.Ships.Remove(ship);
                }
                _context.Players.Remove(player);
            }
            _context.Games.Remove(gameToUpdate);

            try
            {
                await _context.Games.AddAsync(gameAfterShot);
            }
            catch (Exception e) {
            }

            await _context.SaveChangesAsync();
            return gameAfterShot;
        }
    }
}
