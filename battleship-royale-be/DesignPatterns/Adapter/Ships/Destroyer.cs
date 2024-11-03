using battleship_royale_be.DesignPatterns.Adapter;

namespace battleship_royale_be.Models
{
    public class Destroyer : IShip
    {
        public int GetHitPoints() => 5;
    }
}