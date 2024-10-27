using battleship_royale_be.DesignPatterns.AbstractFactory.Boards;
using battleship_royale_be.DesignPatterns.AbstractFactory.Ships;

namespace battleship_royale_be.DesignPatterns.AbstractFactory
{
    public class LevelTwoCreator : LevelCreator
    {
        public Board CreateBoard()
        {
            return new LevelTwoBoard();
        }

        public Ship CreateShip()
        {
            return new LevelTwoShip();
        }
    }
}
