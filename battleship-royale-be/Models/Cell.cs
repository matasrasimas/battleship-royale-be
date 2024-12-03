namespace battleship_royale_be.Models
{
    public class Cell
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsHit { get; set; }
        public bool IsShip { get; set; }
        public bool IsIsland { get; set; }
        public string ImagePath { get; set; }

        public Cell(Guid Id, int Row, int Col, bool IsHit, bool IsShip, bool IsIsland, string ImagePath = "") {
            this.Id = Id;
            this.Row = Row;
            this.Col = Col;
            this.IsHit = IsHit;
            this.IsShip = IsShip;
            this.IsIsland = IsIsland;
            this.ImagePath = ImagePath;
        }
    }
}
