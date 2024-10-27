using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class StandardShotStrategy : IShotStrategy
    {
        public int GetMaxShots(Ship ship)
        {
            return ship.HitPoints switch
            {
                1 => 1,
                2 => 1,
                3 => 2,
                4 => 3,
                5 => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(ship.HitPoints), $"Not expected hit points value: {ship.HitPoints}")
            };
        }
    }
}
