using battleship_royale_be.Data;
using battleship_royale_be.Usecase.Shoot;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Models.Command
{
    public class ShootCommand : ICommand
    {
        // Receiver
        public IShootUseCase _shootUseCase;
        // Backup of the previous state, required for undoing the command.
        private Game? _backup;
        // Context data, required for launching the receiver's methods.
        private Guid _id;
        private ShotCoordinates _shotCoords;
        private string _connectionId;
        private int _shotCount;
        public ShootCommand(IShootUseCase shootUseCase, Guid id, ShotCoordinates shotCoords, string connectionId, int shotCount)
        {
            _shootUseCase = shootUseCase;
            _id = id;
            _shotCoords = shotCoords;
            _connectionId = connectionId;
            _shotCount = shotCount;
        }
        public async Task<Game?> Execute()
        {
            Game afterShot = await _shootUseCase.Shoot(_id, _shotCoords, _connectionId, _shotCount);
            _backup = _shootUseCase.GetBackup();
            return afterShot;
        }

        public async Task<Game?> Undo(BattleshipAPIContext _context, string connId)
        {
            Game? gameToUpdate = await _context.Games
                .Include(game => game.Players)
                    .ThenInclude(player => player.Cells)
                .Include(game => game.Players)
                    .ThenInclude(player => player.Ships)
                       .ThenInclude(ship => ship.Coordinates)
                .Where(g => g.Id == _id)
                .FirstOrDefaultAsync();

            if (gameToUpdate == null || _backup == null ||
                _backup.Players.Where(player => player.IsYourTurn).FirstOrDefault().ConnectionId != connId)
            {
                return null;
            }
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

            await _context.Games.AddAsync(_backup);
            await _context.SaveChangesAsync();

            return _backup;
        }
    }
}