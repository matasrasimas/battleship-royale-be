namespace battleship_royale_be.DesignPatterns.Decorator.ShotPointsChangers
{
    public class HitShipSankAddPoints : PointsCalculatorDecorator
    {
        private int _number;
        public HitShipSankAddPoints(IPointsCalculator pointsCalculator, int number) : base(pointsCalculator)
        {
            _number = number;
        }

        public override int CalculateShotPoints()
        {
            int calculatedShotPoints = base.CalculateShotPoints();
            //TODO better algo
            return calculatedShotPoints + _number;
        }
    }
}
