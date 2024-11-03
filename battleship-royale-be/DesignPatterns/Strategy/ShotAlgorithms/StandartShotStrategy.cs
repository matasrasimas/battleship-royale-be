using battleship_royale_be.DesignPatterns.Adapter;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class StandardShotStrategy : IShotStrategy
    {
        public int GetMaxShots(Ship ship)
        {
            return 0;
        }
    }
}
