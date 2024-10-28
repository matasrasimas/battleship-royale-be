namespace battleship_royale_be.Models
{
    public class Coordinates : ICloneable
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public Coordinates(Guid Id, int Row, int Col)
        {
            this.Id = Id;
            this.Row = Row;
            this.Col = Col;
        }

        public object Clone()
        {
            return (Coordinates)this.MemberwiseClone();
        }
    }
}
