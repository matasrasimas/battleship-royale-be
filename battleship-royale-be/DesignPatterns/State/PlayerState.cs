using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.State
{
    public abstract class PlayerState
    {
        protected Player player;

        public PlayerState(Player player) {
            this.player = player;
        }

        public abstract Game onShoot();
    }
}
