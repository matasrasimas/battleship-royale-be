using battleship_royale_be.DesignPatterns.Composite;

namespace battleship_royale_be.Models
{
    public class Player : Observer.IObserver, IShipComponent
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        public List<Cell> Cells { get; set; }
        public List<Ship> Ships { get; set; }
        public string GameStatus { get; set; }
        public bool IsYourTurn { get; set; }
        public int Points { get; set; }

        public Player() { }

        public Player(Guid Id, string ConnectionId, List<Cell> Cells, List<Ship> Ships, string GameStatus, bool IsYourTurn, int Points)
        {
            this.Id = Id;
            this.ConnectionId = ConnectionId;
            this.Cells = Cells;
            this.Ships = Ships;
            this.GameStatus = GameStatus;
            this.IsYourTurn = IsYourTurn;
            this.Points = Points;
        }

        public void Update(string msg)
        {
            Console.WriteLine(msg);
        }

        // Move ships based on hit points
        public void MoveByHitPoints(int hitPoints)
        {
            foreach (var ship in Ships)
            {
                ship.MoveByHitPoints(hitPoints);
            }
        }

    }
}
