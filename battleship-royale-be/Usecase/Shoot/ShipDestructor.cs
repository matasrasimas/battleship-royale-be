using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShipDestructor
    {
        public static Board DamageShip(Board board, Ship ship)
        {
            var updatedShipsList = new List<Ship>(board.Ships);

            var updatedShip = ShipBuilder
                .From(ship)
                .SetHitPoints(ship.HitPoints - 1)
                .Build();

            var shipIndex = updatedShipsList.IndexOf(ship);
            if (shipIndex != -1)
            {
                updatedShipsList[shipIndex] = updatedShip;
            }

            return BoardBuilder
                .From(board)
                .SetShips(updatedShipsList)
                .Build();
        }

        public static Board DestroyShip(Board board, Ship ship)
        {
            var boardAfterShot = MakeShotsAroundShip(board, ship);
            var updatedShipsList = new List<Ship>(board.Ships);
            updatedShipsList.Remove(ship);

            return BoardBuilder
                .From(boardAfterShot)
                .SetShips(updatedShipsList)
                .Build();
        }

        private static Board MakeShotsAroundShip(Board board, Ship ship)
        {
            return ship.Coordinates
                .Aggregate(board, (currentBoard, targetCoords) => MakeShotsAroundCell(targetCoords, currentBoard));
        }

        private static Board MakeShotsAroundCell(Coordinates coords, Board board)
        {
            var newGrid = (Cell[,])board.CloneGrid().Clone();

            foreach (Coordinates coord in GenerateAdjacentCoordinates(coords))
            {
                if (board.CanShoot(coord))
                {
                    var cell = newGrid[coord.Row, coord.Col];
                    newGrid[coord.Row, coord.Col] = new Cell(Guid.NewGuid(), coord.Row, coord.Col, cell.IsIsland ? false : true, cell.IsShip, cell.IsIsland);
                }
            }

            return BoardBuilder
                .From(board)
                .SetGrid(newGrid)
                .Build();
        }

        private static IEnumerable<Coordinates> GenerateAdjacentCoordinates(Coordinates coords)
        {
            return Enumerable.Range(coords.Row - 1, 3)
                .SelectMany(row => Enumerable.Range(coords.Col - 1, 3)
                .Select(col => new Coordinates(Guid.NewGuid(), row, col)));
        }
    }
}
