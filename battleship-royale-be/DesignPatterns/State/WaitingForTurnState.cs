using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.State
{
    public class WaitingForTurnState : PlayerState
    {
        public WaitingForTurnState(Player player) : base(player)
        {
        }

        public override Game onShoot()
        {
            player.SetState(new WaitingForTurnState(player));
            return player.Shoot();
        }
    }
}
