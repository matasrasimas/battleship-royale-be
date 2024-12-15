using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Adapter_Flyweight
{
    public interface IShip
    {
        int GetHitPoints();
        string GetName();
        string GetImagePath();
    }
}
