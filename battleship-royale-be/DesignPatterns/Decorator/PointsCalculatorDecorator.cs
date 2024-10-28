
namespace battleship_royale_be.DesignPatterns.Decorator
{
    public abstract class PointsCalculatorDecorator : IPointsCalculator
    {
        protected IPointsCalculator _pointsCalculator;

        public PointsCalculatorDecorator(IPointsCalculator pointsCalculator)
        {
            this._pointsCalculator = pointsCalculator;
        }

        public virtual int CalculateShotPoints()
        {
            if (_pointsCalculator != null)
            {
                return this._pointsCalculator.CalculateShotPoints();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
