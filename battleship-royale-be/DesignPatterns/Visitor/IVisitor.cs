using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Visitor
{
    public interface IVisitor
    {
        void Visit(Battleship element);
        void Visit(Carrier element);
        void Visit(Cruiser element);
        void Visit(Destroyer element);
        void Visit(Submarine element);
    }
}
