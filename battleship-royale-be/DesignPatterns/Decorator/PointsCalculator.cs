namespace battleship_royale_be.DesignPatterns.Decorator
{
    public class PointsCalculator : IPointsCalculator
    {
        public int CalculateShotPoints() 
        {
            return 100;
        }
    }
}
