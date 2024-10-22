namespace battleship_royale_be.DesignPatterns.FactoryMethod.Cells
{
    public abstract class Cell
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public Cell(Guid Id, int Row, int Col)
        {
            this.Id = Id;
            this.Row = Row;
            this.Col = Col;
        }
    }
}
