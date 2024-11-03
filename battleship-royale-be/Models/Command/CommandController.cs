using battleship_royale_be.Data;
using Microsoft.AspNetCore.SignalR;

namespace battleship_royale_be.Models.Command
{
    public class CommandController
    {
        private List<ICommand> _commands = [];

        public List<ICommand> GetCommands()
        {
            return _commands;
        }

        public async Task<Game> Run(ICommand cmd)
        {
            _commands.Add(cmd);
            Game game = await cmd.Execute();
            return game;
        }
        public async Task<Game?> Undo(BattleshipAPIContext _context, string connId)
        {
            if (_commands.Count != 0)
            {
                Game game = await _commands.Last().Undo(_context, connId);
                if (game != null) {
                    _commands.Remove(_commands.Last());
                    return game;
                }
                else
                    return null;
            }
            return null;
        }
    }
}