
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
    }
}
