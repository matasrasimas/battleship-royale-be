using battleship_royale_be.DesignPatterns.Visitor;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Adapter_Flyweight
{
    public interface IShip
    {
        int GetHitPoints();
        string GetName();
        string GetImagePath();
        string GetColor();
        void Accept(IVisitor visitor);
    }
}
