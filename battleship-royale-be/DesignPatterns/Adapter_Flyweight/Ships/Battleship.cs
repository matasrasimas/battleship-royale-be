using battleship_royale_be.DesignPatterns.Adapter_Flyweight;

namespace battleship_royale_be.Models
{
    public class Battleship : IShip
    {
        private readonly string imagePath = "/images/battleship.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 2;
        public string GetName() => "Battleship";
    }
}