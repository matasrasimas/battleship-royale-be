using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Template.Strategies
{
    public class SingleShot : ShotStrategy
    {
        protected override int CalculateDamage(Ship ship)
        {
            return 1;
        }
    }
}
