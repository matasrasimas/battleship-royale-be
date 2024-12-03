using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.Models
{
    public class Cruiser : IShip
    {
        private readonly string imagePath = "/images/cruiser.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 4;
        public string GetName() => "Cruiser";
    }
}