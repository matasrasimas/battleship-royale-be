using battleship_royale_be.DesignPatterns.Adapter;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class StandardShotStrategy : IShotStrategy
    {
        public int GetMaxShots(Ship ship)
        {
            var adapter = new ShipWeaponAdapter(ship);
            int damage = adapter.GetDamage(ship);

            return damage switch
            {
                1 => 1,
                2 => 1,
                3 => 2,
                4 => 2,
                5 => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(damage), $"Not expected damage value: {damage}")
            };
        }
    }
}
