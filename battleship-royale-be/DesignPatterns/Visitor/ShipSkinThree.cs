using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Visitor
{
    public class ShipSkinThree : IVisitor
    {
        public void Visit(Battleship element)
        {
            element.Color = "#FF595E";
        }

        public void Visit(Carrier element)
        {
            element.Color = "#8AC926";
        }

        public void Visit(Cruiser element)
        {
            element.Color = "#96A13A";
        }

        public void Visit(Destroyer element)
        {
            element.Color = "#89043D";
        }

        public void Visit(Submarine element)
        {
            element.Color = "#20FC8F";
        }
    }
}
