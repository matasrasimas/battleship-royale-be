using battleship_royale_be.DesignPatterns.AbstractFactory.Boards;
using battleship_royale_be.DesignPatterns.AbstractFactory.Ships;

namespace battleship_royale_be.DesignPatterns.AbstractFactory
{
    public interface LevelCreator
    {
        Board CreateBoard();
        Ship CreateShip();
    }
}
