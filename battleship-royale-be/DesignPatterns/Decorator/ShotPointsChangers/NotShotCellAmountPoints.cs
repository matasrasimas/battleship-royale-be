namespace battleship_royale_be.DesignPatterns.Decorator.ShotPointsChangers
{
    public class NotShotCellAmountPoints : PointsCalculatorDecorator
    {
        private int _number;
        public NotShotCellAmountPoints(IPointsCalculator pointsCalculator, int number) : base(pointsCalculator)
        {
            _number = number;
        }

        public override int CalculateShotPoints()
        {
            int calculatedShotPoints = base.CalculateShotPoints();
            return calculatedShotPoints + (_number + 10) / 2;
        }
    }
}
