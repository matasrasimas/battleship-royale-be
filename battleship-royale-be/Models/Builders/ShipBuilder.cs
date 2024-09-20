namespace battleship_royale_be.Models.Builders
{
    public class ShipBuilder
    {
        private Guid id;
        private int hitPoints;
        private bool isHorizontal;
        private List<Coordinates> coordinates;

        private ShipBuilder() { }

        public static ShipBuilder From(Ship ship)
        {
            ShipBuilder builder = new ShipBuilder
            {
                id = ship.Id,
                hitPoints = ship.HitPoints,
                isHorizontal = ship.IsHorizontal,
                coordinates = new List<Coordinates>(ship.Coordinates)
            };
            return builder;
        }

        public static ShipBuilder DefaultValues()
        {
            ShipBuilder builder = new ShipBuilder
            {
                id = Guid.NewGuid(),
                hitPoints = 3,
                isHorizontal = false,
                coordinates = new List<Coordinates>()
            };
            return builder;
        }

        public ShipBuilder SetHitPoints(int hitPoints)
        {
            this.hitPoints = hitPoints;
            return this;
        }

        public ShipBuilder SetIsHorizontal(bool isHorizontal)
        {
            this.isHorizontal = isHorizontal;
            return this;
        }

        public ShipBuilder SetCoordinates(List<Coordinates> coordinates)
        {
            this.coordinates = new List<Coordinates>(coordinates);
            return this;
        }

        public Ship Build()
        {
            return new Ship(id, hitPoints, isHorizontal, coordinates);
        }
    }
}
