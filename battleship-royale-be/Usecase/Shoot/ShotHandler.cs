using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShotHandler
    {
        public static List<Player> HandleShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords)
        {
            Cell[,] grid = GridConverter.FromListToArray(targetPlayer.Cells);
            Board board = new Board(
                (Cell[,])grid.Clone(),
                new List<Ship>(targetPlayer.Ships)
            );

            if (!board.CanShoot(new Coordinates(Guid.NewGuid(), targetCoords.Row, targetCoords.Col)) || !attackerPlayer.IsYourTurn)
            {
                return new List<Player> {

                    PlayerBuilder
                    .From(attackerPlayer)
                    .Build(),

                    PlayerBuilder
                    .From(targetPlayer)
                    .Build()
                };
            }

            var gridAfterShot = MarkCellAsHit(board, targetCoords);
            var newBoard = BoardBuilder
                .From(board)
                .SetGrid(gridAfterShot)
                .SetShips(board.Ships)
                .Build();

            return HandleShot(attackerPlayer, targetPlayer, targetCoords, newBoard);
        }

        private static Cell[,] MarkCellAsHit(Board board, ShotCoordinates targetCoords)
        {
            var newGrid = (Cell[,])board.Grid.Clone();
            var cell = newGrid[targetCoords.Row, targetCoords.Col];
            newGrid[targetCoords.Row, targetCoords.Col] =
                new Cell(Guid.NewGuid(), targetCoords.Row, targetCoords.Col, true, cell.IsShip, cell.IsIsland);
            return newGrid;
        }

        private static List<Player> HandleShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board)
        {
            var cell = board.Grid[targetCoords.Row, targetCoords.Col];
            if (cell.IsShip)
            {
                return HandleSuccessfulShot(attackerPlayer, targetPlayer, targetCoords, board);
            }
            else
            {
                return HandleMissedShot(attackerPlayer, targetPlayer, board);
            }
        }

        private static List<Player> HandleSuccessfulShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, Board board)
        {
            var targetShip = board.FindShipByCoordinates(targetCoords);
            if (targetShip == null)
            {
                throw new ApplicationException("This part of code should not be reachable");
            }

            var boardAfterShot = targetShip.HitPoints - 1 <= 0
                ? ShipDestructor.DestroyShip(board, targetShip)
                : ShipDestructor.DamageShip(board, targetShip);

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
    }
}
