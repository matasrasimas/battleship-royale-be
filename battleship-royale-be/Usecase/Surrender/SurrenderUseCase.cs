using battleship_royale_be.Data;
using battleship_royale_be.DesignPatterns.Proxy.GameSurrenderer;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Usecase.Shoot;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Usecase.Surrender
{
    public class SurrenderUseCase : ISurrenderUseCase
    {
        private readonly BattleshipAPIContext _context;
        private GameSurrenderer _gameSurrenderer = new ProxyGameSurrenderer();

        public SurrenderUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public async Task<Game?> Surrender(Guid gameId, string connectionId)
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

            Player? playerThatWantsToSurrender = gameToUpdate.Players.Where(player => player.ConnectionId == connectionId).FirstOrDefault();
            if (playerThatWantsToSurrender == null)
                return null;

            Player? playerThatDoesNotWantToSurrender = gameToUpdate.Players.Where(player => player.ConnectionId != connectionId).FirstOrDefault();
            if (playerThatDoesNotWantToSurrender == null)
                return null;

            Game gameAfterSurrender = _gameSurrenderer.SurrenderGame(gameToUpdate, playerThatWantsToSurrender, playerThatDoesNotWantToSurrender);

            foreach (Player player in gameToUpdate.Players)
            {
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

            await _context.Games.AddAsync(gameAfterSurrender);

            await _context.SaveChangesAsync();
            return gameAfterSurrender;
        }
    }
}
