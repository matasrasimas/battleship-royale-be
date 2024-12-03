using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.DesignPatterns.Adapter_Flyweight
{
    public class ShipAdapter
    {
        private readonly IShip _ship;

        public ShipAdapter(IShip ship)
        {
            _ship = ship;
        }

        public int GetHitPoints() => _ship.GetHitPoints();
    }
}
