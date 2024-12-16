using battleship_royale_be.DesignPatterns.Memento;

namespace battleship_royale_be.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public string ShotResultMessage { get; set; }

        public Game() { }

        public Game(Guid Id, List<Player> Players, string ShotResultMessage)
        {
            this.Id = Id;
            this.Players = Players;
            this.ShotResultMessage = ShotResultMessage;
        }

        public IMemento Save()
        {
            return new GameMemento(new Game { 
                Id = Id, 
                Players = Players.Select(item => (Player)item.DeepClone()).ToList(), 
                ShotResultMessage = ShotResultMessage 
            });
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is GameMemento))
            {
                throw new Exception("Unknown memento class");
            }
            Game undoGame = memento.GetState();
            this.Id = undoGame.Id;
            this.Players = undoGame.Players;
            this.ShotResultMessage = undoGame.ShotResultMessage;
        }
    }
}