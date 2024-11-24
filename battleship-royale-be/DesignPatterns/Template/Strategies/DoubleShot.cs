using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Template.Strategies
{
    public class DoubleShot : ShotStrategy
    {
        protected override int CalculateDamage(Ship ship)
        {
            return 2;
        }
    }
}
