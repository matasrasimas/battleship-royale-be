using battleship_royale_be.Data;
using battleship_royale_be.DesignPatterns.Iterator;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Usecase.Shoot;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Usecase.Pause
{
    public class PauseUseCase : IPauseUseCase
    {
        private readonly BattleshipAPIContext _context;
        private Game _backup;

        public PauseUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }
        public Game GetBackup()
        {
            return _backup;
        }
        public async Task<Game?> Pause(Guid gameId, string connectionId)
        {
            Game? gameToUpdate = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync();

            if (gameToUpdate == null)
                return null;

            _backup = GameBuilder.From(gameToUpdate).SetPlayers(gameToUpdate.Players).Build();

            Player? playerThatWantsToPause = gameToUpdate.Players.Where(player => player.ConnectionId == connectionId).FirstOrDefault();
            if (playerThatWantsToPause == null)
                return null;

            Player? playerThatDoesNotWantToPause = gameToUpdate.Players.Where(player => player.ConnectionId != connectionId).FirstOrDefault();
            if (playerThatDoesNotWantToPause == null)
                return null;

            List<Player> playersListAfterPause =
            [
                PlayerBuilder
                  .From(playerThatWantsToPause)
                  .SetGameStatus("PAUSED_HOST")
                  .Build(),

                PlayerBuilder
                  .From(playerThatDoesNotWantToPause)
                  .SetGameStatus("PAUSED")
                  .Build()
            ];

            Game gameAfterPause = GameBuilder.From(gameToUpdate).SetPlayers(playersListAfterPause).Build();

            foreach (Player player in gameToUpdate.Players)
            {
                // Remove all cells associated with the player
                foreach (Cell cell in player.Cells)
                {
                    _context.Cells.Remove(cell);
                }

                // Use ShipIterator to iterate through each ship
                ShipIterator shipIterator = new ShipIterator(player.Ships);
                while (shipIterator.HasNext())
                {
                    Ship ship = shipIterator.Next();

                    // Use CoordinatesIterator to iterate through coordinates of the ship
                    CoordinatesIterator coordsIterator = new CoordinatesIterator(ship.Coordinates);
                    while (coordsIterator.HasNext())
                    {
                        Coordinates coords = coordsIterator.Next();
                        _context.Coordinates.Remove(coords);
                    }

                    _context.Ships.Remove(ship);
                }

                _context.Players.Remove(player);
            }

            _context.Games.Remove(gameToUpdate);

            await _context.Games.AddAsync(gameAfterPause);

            await _context.SaveChangesAsync();
            return gameAfterPause;
        }
    }
}
