using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class DoubleShotStrategy : IShotStrategy
    {
        public int GetDamage(Ship ship)
        {
            return 2;
        }
    }
}
