using battleship_royale_be.DesignPatterns.Adapter;

namespace battleship_royale_be.Models
{
    public class Cruiser : IShip
    {
        public int GetHitPoints() => 4;
    }
}