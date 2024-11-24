using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Template.Strategies
{
    public class TripleShot : ShotStrategy
    {
        protected override int CalculateDamage(Ship ship)
        {
            return 3;
        }
    }
}
