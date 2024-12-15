using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.State
{
    public class LowShipsState : PlayerState
    {
        public LowShipsState(Player player) : base(player)
        {
        }

        public override Game onShoot()
        {
            player.SetState(new LowShipsState(player));
            return player.Shoot();
        }
    }
}
