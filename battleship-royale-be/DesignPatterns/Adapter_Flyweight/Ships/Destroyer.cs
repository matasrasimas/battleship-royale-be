using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.Models
{
    public class Destroyer : IShip
    {
        private readonly string imagePath = "/images/destroyer.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 5;
        public string GetName() => "Destroyer";
    }
}