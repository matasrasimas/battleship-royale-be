using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using battleship_royale_be.DesignPatterns.Visitor;

namespace battleship_royale_be.Models
{
    public class Battleship : IShip
    {
        private readonly string imagePath = "/images/battleship.png";
        public string GetImagePath() => imagePath;
        public int GetHitPoints() => 2;
        public string GetName() => "Battleship";
        public string? Color {  get; set; }
        public string GetColor() => Color;

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}