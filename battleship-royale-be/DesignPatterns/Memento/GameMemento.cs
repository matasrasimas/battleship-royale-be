using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Memento
{
    public class GameMemento : IMemento
    {
        private Game _state;

        public GameMemento(Game state)
        {
            this._state = state;
        }

        public Game GetState()
        {
            return _state;
        }
    }
}
