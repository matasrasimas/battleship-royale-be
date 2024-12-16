using battleship_royale_be.Models;
using System.Text.Json;

namespace battleship_royale_be.DesignPatterns.Memento
{
    public class Caretaker
    {
        public Guid Id { get; set; } = new Guid();
        public string PlayerId { get; set; }
        private List<IMemento> _gameMementos = new List<IMemento>();
        private Game _gameOriginator = null;

        public Caretaker() { }
        public Caretaker(string playerId, Game gameOriginator)
        {
            if (this.PlayerId == null)
            {
                PlayerId = playerId;
            }
            _gameOriginator = gameOriginator;
        }
        public Caretaker(Game gameOriginator)
        {
            _gameOriginator = gameOriginator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            _gameMementos.Add(_gameOriginator.Save());
        }

        public void Undo()
        {
            if (_gameMementos.Count == 0)
            {
                return;
            }

            var memento = _gameMementos.Last();
            _gameMementos.Remove(memento);

            try
            {
                _gameOriginator.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }
    }
}
