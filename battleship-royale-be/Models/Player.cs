using battleship_royale_be.DesignPatterns.State;
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
        public int ShotsRemaining { get; set; }
        private PlayerState state;


        public Player() { }

        public Player(Guid Id, string ConnectionId, List<Cell> Cells, List<Ship> Ships, string GameStatus, bool IsYourTurn, int Points, int ShotsRemaining)
        {
            this.Id = Id;
            this.ConnectionId = ConnectionId;
            this.Cells = Cells;
            this.Ships = Ships;
            this.GameStatus = GameStatus;
            this.IsYourTurn = IsYourTurn;
            this.Points = Points;
            this.ShotsRemaining = ShotsRemaining;
        }

        public Player DeepClone()
        {
            return new Player
            {
                Id = Id,
                ConnectionId = ConnectionId,
                Cells = Cells.Select(item => (Cell)item.DeepClone()).ToList(),
                Ships = Ships.Select(item => (Ship)item.Clone()).ToList(),
                GameStatus = GameStatus,
                IsYourTurn = IsYourTurn,
                Points = Points
            };
        }

        public void Update(string msg)
        {
            Console.WriteLine(msg);
        }


        public void SetState(PlayerState state) {
            this.state = state;
        }

        public Game Shoot() {
            throw new NotImplementedException();
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
