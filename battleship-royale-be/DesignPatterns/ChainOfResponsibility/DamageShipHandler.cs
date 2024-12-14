using battleship_royale_be.DesignPatterns.Decorator.ShotPointsChangers;
using battleship_royale_be.DesignPatterns.Decorator;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.Usecase.Shoot;

namespace battleship_royale_be.DesignPatterns.ChainOfResponsibility
{
    public class DamageShipHandler : Handler
    {
        public override List<Player> Handle(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount, Dictionary<Guid, int> shotsFired, Cell[,] grid, Board board, Ship targetShip)
        {
            var newGrid = MarkCellAsHit(board, targetCoords);
            var newBoard = new Board(newGrid, new List<Ship>(board.Ships));

            return HandleSuccessfulShot(attackerPlayer, targetPlayer, targetCoords, newBoard, shotCount, targetShip, shotsFired, grid);
        }

        private static Cell[,] MarkCellAsHit(Board board, ShotCoordinates targetCoords)
        {
            var newGrid = (Cell[,])board.Grid.Clone();
            var cell = newGrid[targetCoords.Row, targetCoords.Col];
            newGrid[targetCoords.Row, targetCoords.Col] =
                new Cell(Guid.NewGuid(), targetCoords.Row, targetCoords.Col, true, cell.IsShip, cell.IsIsland, cell.ImagePath);
            return newGrid;
        }

        private List<Player> HandleSuccessfulShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board, int shotCount, Ship targetShip, Dictionary<Guid, int> shotsFired, Cell[,] grid)
        {
            PointsCalculator pointsCalculator = new PointsCalculator();
            ShotSizePoints shotSizePoints = new ShotSizePoints(pointsCalculator, shotCount);
            NotShotCellAmountPoints notShotCellAmountPoints = new NotShotCellAmountPoints(shotSizePoints, board.CanShootCellAmount());

            if (targetShip == null)
            {
                throw new ApplicationException("This part of code should not be reachable");
            }

            if (targetShip.HitPoints <= 0)
            {
                throw new ApplicationException("Cannot hit a destroyed ship.");
            }

            targetShip.HitPoints -= shotCount;

            bool isDestroyed = targetShip.HitPoints <= 0;

            if (isDestroyed)
            {       
                return HandleNext(attackerPlayer, targetPlayer, targetCoords, shotCount, shotsFired, grid, board, targetShip);
            }
            else
            {
                int calculatedPoints = notShotCellAmountPoints.CalculateShotPoints();
                var boardAfterShot = board;

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

        private static List<Player> HandleTurnEnd(Player attackerPlayer, Player targetPlayer, Board board, Dictionary<Guid, int> shotsFired)
        {
            shotsFired[attackerPlayer.Id] = 0;
            return new List<Player> {
                PlayerBuilder
                .From(attackerPlayer)
                .SetIsYourTurn(false)
                .Build(),

                PlayerBuilder
                .From(ShipsMover.MoveShips(targetPlayer, new List<Ship>(board.Ships), board.Grid))
                .SetGameStatus("IN_PROGRESS")
                .SetIsYourTurn(true)
                .Build()
            };
        }
    }
}
