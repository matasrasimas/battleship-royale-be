using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShotHandler
    {
        public static Game HandleShot(Game game, ShotCoordinates targetCoords)
        {
            Cell[,] grid = GridConverter.FromListToArray(game.Cells);
            Board board = new Board(
                (Cell[,])grid.Clone(),
                new List<Ship>(game.Ships)
            );

            if (!board.CanShoot(new Coordinates(Guid.NewGuid(), targetCoords.Row, targetCoords.Col)))
            {
                return GameBuilder
                    .From(game)
                    .SetCells(GridConverter.FromArrayToList(board.Grid))
                    .SetShips(new List<Ship>(board.Ships))
                    .SetShotResultMessage("Invalid shot!")
                    .Build();
            }

            var gridAfterShot = MarkCellAsHit(board, targetCoords);
            var newBoard = BoardBuilder
                .From(board)
                .SetGrid(gridAfterShot)
                .SetShips(board.Ships)
                .Build();

            return HandleShot(game, targetCoords, newBoard);
        }

        private static Cell[,] MarkCellAsHit(Board board, ShotCoordinates targetCoords)
        {
            var newGrid = (Cell[,])board.Grid.Clone();
            var cell = newGrid[targetCoords.Row, targetCoords.Col];
            newGrid[targetCoords.Row, targetCoords.Col] =
                new Cell(Guid.NewGuid(), targetCoords.Row, targetCoords.Col, true, cell.IsShip);
            return newGrid;
        }

        private static Game HandleShot(Game targetGame, ShotCoordinates targetCoords, Board board)
        {
            var cell = board.Grid[targetCoords.Row, targetCoords.Col];
            if (cell.IsShip)
            {
                return HandleSuccessfulShot(targetGame, targetCoords, board);
            }
            else
            {
                return HandleMissedShot(targetGame, board);
            }
        }

        private static Game HandleSuccessfulShot(Game game, ShotCoordinates targetCoords, Board board)
        {
            var targetShip = board.FindShipByCoordinates(targetCoords);
            if (targetShip == null)
            {
                return GameBuilder
                    .From(game)
                    .SetCells(GridConverter.FromArrayToList(board.Grid))
                    .SetShips(new List<Ship>(board.Ships))
                    .Build();
            }

            var boardAfterShot = targetShip.HitPoints - 1 <= 0
                ? ShipDestructor.DestroyShip(board, targetShip)
                : ShipDestructor.DamageShip(board, targetShip);

            string status = !boardAfterShot.Ships.Any()
                ? "WON"
                : game.Status;

            return GameBuilder
                .From(game)
                .SetCells(GridConverter.FromArrayToList(boardAfterShot.Grid))
                .SetShips(new List<Ship>(boardAfterShot.Ships))
                .SetShotResultMessage("You've hit a ship!")
                .SetStatus(status)
                .Build();
        }

        private static Game HandleMissedShot(Game game, Board board)
        {
            return GameBuilder
                .From(game)
                .SetCells(GridConverter.FromArrayToList(board.Grid))
                .SetShips(new List<Ship>(board.Ships))
                .SetShotResultMessage("You've missed!")
                .SetStatus("IN_PROGRESS")
                .Build();
        }
    }
}
