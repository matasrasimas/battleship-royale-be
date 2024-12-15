using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.Models
{
    public class Submarine : Ship, IShip
    {
        private readonly string imagePath = "/images/submarine.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 3;
        public string GetName() => "Submarine";
    }
}