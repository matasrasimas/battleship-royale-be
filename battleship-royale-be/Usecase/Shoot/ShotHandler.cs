using battleship_royale_be.DesignPatterns.Strategy;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.DesignPatterns.Decorator;
using battleship_royale_be.DesignPatterns.Decorator.ShotPointsChangers;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShotHandler
    {
        private static Dictionary<Guid, int> shotsFired = new Dictionary<Guid, int>();
        private static IShotStrategy shotStrategy = new StandardShotStrategy();

        public static List<Player> HandleShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount)
        {
            Cell[,] grid = GridConverter.FromListToArray(targetPlayer.Cells);
            Board board = new Board(
                (Cell[,])grid.Clone(),
                new List<Ship>(targetPlayer.Ships)
            );

            if (!shotsFired.ContainsKey(attackerPlayer.Id))
            {
                shotsFired[attackerPlayer.Id] = 0;
            }

            if (!board.CanShoot(new Coordinates(Guid.NewGuid(), targetCoords.Row, targetCoords.Col)) || !attackerPlayer.IsYourTurn)
            {
                return new List<Player> {
                    PlayerBuilder.From(attackerPlayer).Build(),
                    PlayerBuilder.From(targetPlayer).Build()
                };
            }

            var attackingShip = attackerPlayer.Ships.FirstOrDefault(ship => ship.HitPoints > 0);
            if (attackingShip == null)
            {
                throw new ApplicationException("No available ships to attack.");
            }

            return HandleActualShot(attackerPlayer, targetPlayer, targetCoords, board, shotCount);
        }

        private static List<Player> HandleActualShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board, int shotCount)
        {
            var cell = board.Grid[targetCoords.Row, targetCoords.Col];
            bool isShip = cell.IsShip;

            if (isShip)
            {
                var newGrid = MarkCellAsHit(board, targetCoords);
                var newBoard = new Board(newGrid, new List<Ship>(board.Ships));

                return HandleSuccessfulShot(attackerPlayer, targetPlayer, targetCoords, newBoard, shotCount);
            }
            else
            {
                var newGrid = MarkCellAsHit(board, targetCoords);
                var newBoard = new Board(newGrid, new List<Ship>(board.Ships));
                shotsFired[attackerPlayer.Id]++;
                int maxShots = shotStrategy.GetMaxShots(attackerPlayer.Ships.FirstOrDefault(ship => ship.HitPoints > 0));

                if (shotsFired[attackerPlayer.Id] >= maxShots)
                {
                    Console.WriteLine($"Player {attackerPlayer.Id} has reached maximum shots. Ending turn.");
                    return HandleTurnEnd(attackerPlayer, targetPlayer, newBoard);
                }

                return HandleMissedShot(attackerPlayer, targetPlayer, newBoard);
            }
        }

        private static Cell[,] MarkCellAsHit(Board board, ShotCoordinates targetCoords)
        {
            var newGrid = (Cell[,])board.Grid.Clone();
            var cell = newGrid[targetCoords.Row, targetCoords.Col];
            newGrid[targetCoords.Row, targetCoords.Col] =
                new Cell(Guid.NewGuid(), targetCoords.Row, targetCoords.Col, true, cell.IsShip, cell.IsIsland);
            return newGrid;
        }

        private static List<Player> HandleSuccessfulShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board, int shotCount)
        {
            PointsCalculator pointsCalculator = new PointsCalculator();
            ShotSizePoints shotSizePoints = new ShotSizePoints(pointsCalculator, shotCount);
            NotShotCellAmountPoints notShotCellAmountPoints = new NotShotCellAmountPoints(shotSizePoints, board.CanShootCellAmount());
            var targetShip = board.FindShipByCoordinates(targetCoords);
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

            int calculatedPoints;
            if (isDestroyed)
            {
                HitShipSankAddPoints hitShipSankAddPoints = new HitShipSankAddPoints(notShotCellAmountPoints, 200);
                calculatedPoints = hitShipSankAddPoints.CalculateShotPoints();
            }
            else
            {
                calculatedPoints = notShotCellAmountPoints.CalculateShotPoints();
            }

            var boardAfterShot = isDestroyed 
                ? ShipDestructor.DestroyShip(board, targetShip) 
                : board;

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
                .SetShips(new List<Ship>(boardAfterShot.Ships))
                .SetGameStatus(isDefeated ? "LOST" : "IN_PROGRESS")
                .Build()
            };
        }

        private static List<Player> HandleMissedShot(Player attackerPlayer, Player targetPlayer, Board board)
        {
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

        private static List<Player> HandleTurnEnd(Player attackerPlayer, Player targetPlayer, Board board)
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
