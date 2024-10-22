namespace battleship_royale_be.DesignPatterns.FactoryMethod.Cells
{
    public class WaterCell : Cell
    {
        public bool IsHit { get; set; }
        public bool IsShip { get; set; }

        public WaterCell(Guid Id, int Row, int Col, bool IsHit, bool IsShip) : base(Id, Row, Col)
        {
            {
                this.IsHit = IsHit;
                this.IsShip = IsShip;
            }
        }
    }
}
