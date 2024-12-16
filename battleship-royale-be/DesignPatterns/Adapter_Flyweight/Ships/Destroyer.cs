using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using battleship_royale_be.DesignPatterns.Visitor;

namespace battleship_royale_be.Models
{
    public class Destroyer : IShip
    {
        private readonly string imagePath = "/images/destroyer.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 5;
        public string GetName() => "Destroyer";
        public string? Color { get; set; }
        public string GetColor() => Color;
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}