using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Visitor
{
    public class ShipSkinTwo : IVisitor
    {
        public void Visit(Battleship element)
        {
            element.Color = "#DC9D00";
        }

        public void Visit(Carrier element)
        {
            element.Color = "#FF006E";
        }

        public void Visit(Cruiser element)
        {
            element.Color = "#8338EC";
        }

        public void Visit(Destroyer element)
        {
            element.Color = "#632B30";
        }

        public void Visit(Submarine element)
        {
            element.Color = "#2274A5";
        }
    }
}
