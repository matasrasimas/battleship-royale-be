using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Adapter
{
    public class StandardWeapon : IWeapon
    {
        public int GetDamage(Ship ship)
        {
            return ship.HitPoints switch
            {
                1 => 1,
                2 => 2,
                3 => 3,
                4 => 4,
                5 => 5,
                _ => throw new ArgumentOutOfRangeException(nameof(ship.HitPoints), $"Not expected hit points value: {ship.HitPoints}")
            };
        }
    }
}
