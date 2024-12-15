using battleship_royale_be.Data;
using battleship_royale_be.Usecase.Move;
using battleship_royale_be.Usecase.Shoot;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Models.Command
{
    public class MoveCommand : ICommand
    {
        // Receiver
        public IMoveUseCase _moveUseCase;
        // Backup of the previous state, required for undoing the command.
        private Game? _backup;
        // Context data, required for launching the receiver's methods.
        private Guid _id;
        private string _connectionId;
        private int _hitPoints;
        public MoveCommand(IMoveUseCase moveUseCase, Guid id, string connectionId, int hitPoints)
        {
            _moveUseCase = moveUseCase;
            _id = id;
            _connectionId = connectionId;
            _hitPoints = hitPoints;
        }
        public async Task<Game?> Execute()
        {
            Game afterMove = await _moveUseCase.Move(_id, _connectionId, _hitPoints);
            _backup = _moveUseCase.GetBackup();
            return afterMove;
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