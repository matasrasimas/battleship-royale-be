namespace battleship_royale_be.Models
{
    public class Ship
    {
        public Guid Id { get; set; }
        public int HitPoints { get; set; }
        public bool IsHorizontal { get; set; }
        public bool CanMove { get; set; }
        public List<Coordinates> Coordinates { get; set; }

        public Ship() { }

        public Ship(Guid Id, int HitPoints, bool IsHorizontal, bool CanMove, List<Coordinates> Coordinates)
        {
            this.Id = Id;
            this.HitPoints = HitPoints;
            this.IsHorizontal = IsHorizontal;
            this.CanMove = CanMove;
            this.Coordinates = Coordinates;
        }

        public Coordinates calculateStartCoordinates()
        {
            return IsHorizontal
                ? Coordinates.OrderBy(coords => coords.Col).First()
                : Coordinates.OrderBy(coords => coords.Row).First();
        }

        public Coordinates CalculateEndCoordinates(Coordinates startCoords)
        {
            int endRow = CalculateShipEndRow(startCoords);
            int endCol = CalculateShipEndColumn(startCoords);
            return new Coordinates(Guid.NewGuid(), endRow, endCol);
        }

        private int CalculateShipEndColumn(Coordinates startCoords)
        {
            return IsHorizontal ? startCoords.Col + HitPoints - 1 : startCoords.Col;
        }

        private int CalculateShipEndRow(Coordinates startCoords)
        {
            return IsHorizontal ? startCoords.Row : startCoords.Row + HitPoints - 1;
        }
    }
}
