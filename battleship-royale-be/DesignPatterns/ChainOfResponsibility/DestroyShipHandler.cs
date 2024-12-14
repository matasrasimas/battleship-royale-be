using battleship_royale_be.DesignPatterns.Decorator;
using battleship_royale_be.DesignPatterns.Decorator.ShotPointsChangers;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.Usecase.Shoot;

namespace battleship_royale_be.DesignPatterns.ChainOfResponsibility
{
    public class DestroyShipHandler : Handler
    {
        public override List<Player> Handle(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount, Dictionary<Guid, int> shotsFired, Cell[,] grid, Board board, Ship targetShip)
        {
            PointsCalculator pointsCalculator = new PointsCalculator();
            ShotSizePoints shotSizePoints = new ShotSizePoints(pointsCalculator, shotCount);
            NotShotCellAmountPoints notShotCellAmountPoints = new NotShotCellAmountPoints(shotSizePoints, board.CanShootCellAmount());
            HitShipSankAddPoints hitShipSankAddPoints = new HitShipSankAddPoints(notShotCellAmountPoints, 200);
            int calculatedPoints = hitShipSankAddPoints.CalculateShotPoints();

            var boardAfterShot = ShipDestructor.DestroyShip(board, targetShip);

            bool isDefeated = !boardAfterShot.Ships.Any();

            return new List<Player> {
                PlayerBuilder
                .From(attackerPlayer)
                .SetGameStatus(isDefeated ? "WON" : "IN_PROGRESS")
                .SetPoints(attackerPlayer.Points + calculatedPoints)
                .Build(),

                PlayerBuilder
                .From(targetPlayer)
                .SetCells(GridConverter.FromArrayToList(boardAfterShot.Grid))
                .SetShips(boardAfterShot.Ships.Select(item => (Ship)item.Clone()).ToList())
                .SetGameStatus(isDefeated ? "LOST" : "IN_PROGRESS")
                .Build()
            };
        }
    }
}
