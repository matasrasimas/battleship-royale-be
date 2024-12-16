using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using battleship_royale_be.DesignPatterns.Visitor;

namespace battleship_royale_be.Models
{
    public class Cruiser : IShip
    {
        private readonly string imagePath = "/images/cruiser.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 4;
        public string GetName() => "Cruiser";
        public string? Color { get; set; }
        public string GetColor() => Color;
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}