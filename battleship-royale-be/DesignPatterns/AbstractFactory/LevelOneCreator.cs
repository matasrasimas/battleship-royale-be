using battleship_royale_be.DesignPatterns.AbstractFactory.Boards;
using battleship_royale_be.DesignPatterns.AbstractFactory.Ships;

namespace battleship_royale_be.DesignPatterns.AbstractFactory
{
    public class LevelOneCreator : LevelCreator
    {
        public Board CreateBoard()
        {
            return new LevelOneBoard();
        }

        public Ship CreateShip()
        {
            return new LevelOneShip();
        }
    }
}
