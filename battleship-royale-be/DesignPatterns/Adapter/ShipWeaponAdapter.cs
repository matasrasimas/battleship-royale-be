using battleship_royale_be.DesignPatterns.Adapter;
using battleship_royale_be.Models;

    public class ShipWeaponAdapter : IWeapon
    {
        private readonly Ship _ship;

        public ShipWeaponAdapter(Ship ship)
        {
            _ship = ship ?? throw new ArgumentNullException(nameof(ship));
        }

        public void Fire(int targetX, int targetY)
        {
            //TODO
        }
    }

