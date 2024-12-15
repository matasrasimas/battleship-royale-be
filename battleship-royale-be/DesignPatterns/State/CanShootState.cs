using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.State
{
    public class CanAttackState : PlayerState
    {
        public CanAttackState(Player player) : base(player)
        {
        }

        public override Game onShoot()
        {
            player.SetState(new CanAttackState(player));
            return player.Shoot();
        }
    }
}
