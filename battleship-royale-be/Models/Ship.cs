namespace battleship_royale_be.Models
{
    public class Ship : ICloneable
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

        public object ShallowClone()
        {
            return (Ship)this.MemberwiseClone();
        }

        public object Clone()
        {
            Ship clone = (Ship) this.MemberwiseClone();
            Console.WriteLine("Current ship " + this.GetHashCode());
            Console.WriteLine("Current ship coordinates " + this.Coordinates.GetHashCode());
            Console.WriteLine("Cloned ship shallow " + clone.GetHashCode());
            Console.WriteLine("Cloned ship coordinates with shallow " + clone.Coordinates.GetHashCode());
            clone.Coordinates = this.Coordinates.Select(item => (Coordinates)item.Clone()).ToList();
            Console.WriteLine("Cloned ship deep " + clone.GetHashCode());
            Console.WriteLine("Cloned ship coordinates with deep " + clone.Coordinates.GetHashCode());
            return clone;
        }
    }
}
