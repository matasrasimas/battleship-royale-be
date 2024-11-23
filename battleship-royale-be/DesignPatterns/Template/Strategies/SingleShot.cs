using battleship_royale_be.DesignPatterns.AbstractFactory.Ships;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class SingleShot : ShotStrategy
    {
        protected override int CalculateDamage(Ship ship)
        {
            return 1;
        }
    }
}
