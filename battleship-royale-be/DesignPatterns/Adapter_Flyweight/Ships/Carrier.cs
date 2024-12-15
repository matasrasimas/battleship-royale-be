using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.Models
{
    public class Carrier : IShip
    {
        private readonly string imagePath = "/images/carrier.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 1;
        public string GetName() => "Carrier";
    }
}