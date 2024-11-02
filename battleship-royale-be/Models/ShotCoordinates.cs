namespace battleship_royale_be.Models
{
    public class ShotCoordinates
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public ShotCoordinates(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}