using battleship_royale_be.Models;
namespace battleship_royale_be.DesignPatterns.Composite;

public interface IShipComponent
{
    void MoveByHitPoints(int hitPoints);
}
