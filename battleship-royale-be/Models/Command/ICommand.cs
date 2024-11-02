using battleship_royale_be.Data;

namespace battleship_royale_be.Models.Command
{
    // The Command interface declares a method for executing a command.
    public interface ICommand
    {
        Task<Game?> Execute();

        Task<Game?> Undo(BattleshipAPIContext _context, string connId);
    }
}