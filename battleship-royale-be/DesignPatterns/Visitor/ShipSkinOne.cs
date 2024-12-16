using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Visitor
{
    public class ShipSkinOne : IVisitor
    {
        public void Visit(Battleship element)
        {
            element.Color = "#922B3E";
        }

        public void Visit(Carrier element)
        {
            element.Color = "#4A192C";
        }

        public void Visit(Cruiser element)
        {
            element.Color = "#FF7514";
        }

        public void Visit(Destroyer element)
        {
            element.Color = "#826C34";
        }

        public void Visit(Submarine element)
        {
            element.Color = "#1C542D";
        }
    }
}
