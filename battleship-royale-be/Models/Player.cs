
namespace battleship_royale_be.Models
{
    public class Player : Observer.IObserver
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        public List<Cell> Cells { get; set; }
        public List<Ship> Ships { get; set; }
        public string GameStatus { get; set; }
        public bool IsYourTurn { get; set; }

        public Player() { }

        public Player(Guid Id, string ConnectionId, List<Cell> Cells, List<Ship> Ships, string GameStatus, bool IsYourTurn)
        {
            this.Id = Id;
            this.ConnectionId = ConnectionId;
            this.Cells = Cells;
            this.Ships = Ships;
            this.GameStatus = GameStatus;
            this.IsYourTurn = IsYourTurn;
        }

        public void Update(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
