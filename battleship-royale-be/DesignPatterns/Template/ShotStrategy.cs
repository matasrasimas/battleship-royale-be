using battleship_royale_be.DesignPatterns.AbstractFactory.Ships;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public abstract class ShotStrategy
    {
        // Template method defining the general algorithm
        public int GetDamage(Ship ship)
        {
            return CalculateDamage(ship);
        }

        // Abstract method that subclasses must implement
        protected abstract int CalculateDamage(Ship ship);
    }
}
