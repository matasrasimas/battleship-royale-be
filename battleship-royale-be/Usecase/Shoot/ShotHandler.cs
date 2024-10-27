using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShotHandler
    {
        private static Dictionary<Guid, int> shotsFired = new Dictionary<Guid, int>();

        public static List<Player> HandleShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords)
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

            // Get the attacking ship's max damage
            var attackingShip = attackerPlayer.Ships.FirstOrDefault(ship => ship.HitPoints > 0);
            int shotPower = GetMaxShotsForShip(attackingShip);

            return HandleActualShot(attackerPlayer, targetPlayer, targetCoords, board, shotPower);
        }

        private static List<Player> HandleActualShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board, int shotPower)
        {
            var cell = board.Grid[targetCoords.Row, targetCoords.Col];
            bool isHit = cell.IsShip;

            if (isHit)
            {
                // Mark the cell as hit on the board
                var newGrid = MarkCellAsHit(board, targetCoords);
                var newBoard = new Board(newGrid, new List<Ship>(board.Ships)); // Create a new board instance with the updated grid

                return HandleSuccessfulShot(attackerPlayer, targetPlayer, targetCoords, newBoard, shotPower);
            }
            else
            {
                shotsFired[attackerPlayer.Id]++;
                int maxShots = GetMaxShotsForShip(attackerPlayer.Ships.FirstOrDefault(ship => ship.HitPoints > 0));

                if (shotsFired[attackerPlayer.Id] >= maxShots)
                {
                    Console.WriteLine($"Player {attackerPlayer.Id} has reached maximum shots. Ending turn.");
                    return HandleTurnEnd(attackerPlayer, targetPlayer, board);
                }

                return HandleMissedShot(attackerPlayer, targetPlayer, board);
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

        private static List<Player> HandleSuccessfulShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board, int shotPower)
        {
            var targetShip = board.FindShipByCoordinates(targetCoords);
            if (targetShip == null)
            {
                throw new ApplicationException("This part of code should not be reachable");
            }

            // Check if the shot power exceeds the ship's hit points
            if (targetShip.HitPoints <= 0)
            {
                throw new ApplicationException("Cannot hit a destroyed ship.");
            }

            // Logic for immediate destruction of ships with 1 or 2 hit points
            if (targetShip.HitPoints <= 2)
            {
                // Immediately destroy the ship
                var boardAfterShot = ShipDestructor.DestroyShip(board, targetShip);
                bool isDefeated = !boardAfterShot.Ships.Any();

                return new List<Player> {
                    PlayerBuilder
                    .From(attackerPlayer)
                    .SetGameStatus(isDefeated ? "WON" : "IN_PROGRESS")
                    .Build(),

                    PlayerBuilder
                    .From(targetPlayer)
                    .SetCells(GridConverter.FromArrayToList(boardAfterShot.Grid))
                    .SetShips(new List<Ship>(boardAfterShot.Ships))
                    .SetGameStatus(isDefeated ? "LOST" : "IN_PROGRESS")
                    .Build()
                };
            }
            else
            {
                // Damage the ship
                targetShip.HitPoints -= shotPower;

                // Check if the ship should be destroyed after the hit
                var boardAfterShot = targetShip.HitPoints <= 0
                    ? ShipDestructor.DestroyShip(board, targetShip)
                    : board;

                bool isDefeated = !boardAfterShot.Ships.Any();

                return new List<Player> {
                    PlayerBuilder
                    .From(attackerPlayer)
                    .SetGameStatus(isDefeated ? "WON" : "IN_PROGRESS")
                    .Build(),

                    PlayerBuilder
                    .From(targetPlayer)
                    .SetCells(GridConverter.FromArrayToList(boardAfterShot.Grid))
                    .SetShips(new List<Ship>(boardAfterShot.Ships))
                    .SetGameStatus(isDefeated ? "LOST" : "IN_PROGRESS")
                    .Build()
                };
            }
        }

        private static List<Player> HandleMissedShot(Player attackerPlayer, Player targetPlayer, Board board)
        {
            return new List<Player> {
                PlayerBuilder
                .From(attackerPlayer)
                .SetIsYourTurn(false)
                .Build(),

                PlayerBuilder
                .From(targetPlayer)
                .SetCells(GridConverter.FromArrayToList(board.Grid))
                .SetShips(new List<Ship>(board.Ships))
                .SetGameStatus("IN_PROGRESS")
                .SetIsYourTurn(true)
                .Build()
            };
        }

        private static int GetMaxShotsForShip(Ship ship)
        {
            return ship.HitPoints switch
            {
                1 => 1,
                2 => 1,
                3 => 2,
                4 => 2,
                5 => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(ship.HitPoints), $"Not expected hit points value: {ship.HitPoints}")
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
                .From(targetPlayer)
                .SetCells(GridConverter.FromArrayToList(board.Grid))
                .SetShips(new List<Ship>(board.Ships))
                .SetGameStatus("IN_PROGRESS")
                .SetIsYourTurn(true)
                .Build()
            };
        }
    }
}
