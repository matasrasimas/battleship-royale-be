using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.DesignPatterns.Template.Strategies;
using battleship_royale_be.DesignPatterns.Template;

namespace battleship_royale_be.DesignPatterns.ChainOfResponsibility
{
    public class PerformShotHandler : Handler
    {
        public override List<Player> Handle(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount,
            Dictionary<Guid, int> shotsFired, Cell[,] grid, Models.Board board, Ship targetShip)
        {
            var attackingShip = attackerPlayer.Ships.FirstOrDefault(ship => ship.HitPoints > 0);
            if (attackingShip == null)
            {
                throw new ApplicationException("No available ships to attack.");
            }

            var cell = board.Grid[targetCoords.Row, targetCoords.Col];
            bool isShip = cell.IsShip;

            if (isShip)
            {
                return HandleNext(attackerPlayer, targetPlayer, targetCoords, shotCount, shotsFired, grid, board, targetShip);
            }
            else
            {
                var newGrid = MarkCellAsHit(board, targetCoords);
                var newBoard = new Models.Board(newGrid, new List<Ship>(board.Ships));
                shotsFired[attackerPlayer.Id]++;

                // Use the template method to calculate damage
                int maxShots = GetShotStrategy(shotsFired[attackerPlayer.Id]).GetDamage(attackingShip);

                if (shotsFired[attackerPlayer.Id] >= maxShots)
                {
                    Console.WriteLine($"Player {attackerPlayer.Id} has reached maximum shots. Ending turn.");
                    return HandleTurnEnd(attackerPlayer, targetPlayer, newBoard, shotsFired);
                }

                return HandleMissedShot(attackerPlayer, targetPlayer, newBoard);
            }
        }

        private static Cell[,] MarkCellAsHit(Models.Board board, ShotCoordinates targetCoords)
        {
            var newGrid = (Cell[,])board.Grid.Clone();
            var cell = newGrid[targetCoords.Row, targetCoords.Col];
            newGrid[targetCoords.Row, targetCoords.Col] =
                new Cell(Guid.NewGuid(), targetCoords.Row, targetCoords.Col, true, cell.IsShip, cell.IsIsland, cell.ImagePath, cell.Color);
            return newGrid;
        }

        private static List<Player> HandleMissedShot(Player attackerPlayer, Player targetPlayer, Models.Board board)
        {
            int shotsRemaining = attackerPlayer.ShotsRemaining - 1;
            return new List<Player> {
                PlayerBuilder
                .From(attackerPlayer)
                .SetIsYourTurn(shotsRemaining != 0 && shotsRemaining != -1)
                .SetShotsRemaining(shotsRemaining)
                .Build(),

                PlayerBuilder
                .From(ShipsMover.MoveShips(targetPlayer, new List<Ship>(board.Ships), board.Grid))
                .SetGameStatus("IN_PROGRESS")
                .SetIsYourTurn(shotsRemaining <= 0)
                .SetShotsRemaining(targetPlayer.Ships.Count <= 2 ? 2 : 1)
                .Build()
            };
        }

        private static List<Player> HandleTurnEnd(Player attackerPlayer, Player targetPlayer, Models.Board board, Dictionary<Guid, int> shotsFired)
        {
            shotsFired[attackerPlayer.Id] = 0;
            int shotsRemaining = attackerPlayer.ShotsRemaining - 1;
            return new List<Player> {
                PlayerBuilder
                .From(attackerPlayer)
                .SetIsYourTurn(shotsRemaining != 0)
                .SetShotsRemaining(shotsRemaining)
                .Build(),

                PlayerBuilder
                .From(ShipsMover.MoveShips(targetPlayer, new List<Ship>(board.Ships), board.Grid))
                .SetGameStatus("IN_PROGRESS")
                .SetIsYourTurn(shotsRemaining == 0)
                .SetShotsRemaining(targetPlayer.Ships.Count <= 2 ? 2 : 1)
                .Build()
            };
        }

        private static ShotStrategy GetShotStrategy(int shotCount)
        {
            return shotCount switch
            {
                1 => new SingleShot(),
                2 => new DoubleShot(),
                3 => new TripleShot(),
                _ => throw new ArgumentException("Invalid shot count")
            };
        }
    }
}
