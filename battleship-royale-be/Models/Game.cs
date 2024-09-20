namespace battleship_royale_be.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public List<Cell> Cells { get; set; }
        public List<Ship> Ships { get; set; }
        public string ShotResultMessage { get; set; }
        public string Status { get; set; }

        public Game() { }

        public Game(Guid Id, List<Cell> Cells, List<Ship> Ships, string ShotResultMessage, string Status)
        {
            this.Id = Id;
            this.Cells = Cells;
            this.Ships = Ships;
            this.ShotResultMessage = ShotResultMessage;
            this.Status = Status;
        }
    }
}
