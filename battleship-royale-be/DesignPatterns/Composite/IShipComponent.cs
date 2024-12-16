using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Composite
{
    public interface IShipComponent
    {
        Board Damage(Board board);
        Board Destroy(Board board);
    }
}
