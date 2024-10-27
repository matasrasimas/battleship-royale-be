using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Adapter
{
    public class ShipWeaponAdapter : IWeapon
    {
        private readonly Ship _ship;

        public ShipWeaponAdapter(Ship ship)
        {
            _ship = ship ?? throw new ArgumentNullException(nameof(ship));
        }

        public int GetDamage(Ship ship)
        {
            IWeapon weapon = new StandardWeapon();
            return weapon.GetDamage(ship);
        }
    }
}
