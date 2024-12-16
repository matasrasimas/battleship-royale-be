using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using battleship_royale_be.DesignPatterns.Visitor;

namespace battleship_royale_be.Models
{
    public class Carrier : IShip
    {
        private readonly string imagePath = "/images/carrier.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 1;
        public string GetName() => "Carrier";
        public string? Color { get; set; }
        public string GetColor() => Color;
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}