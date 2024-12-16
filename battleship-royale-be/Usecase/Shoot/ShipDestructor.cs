using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models;
using battleship_royale_be.DesignPatterns.Composite;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShipDestructor
    {
        // This method damages the ship (or group of ships)
        public static Board DamageShip(Board board, IShipComponent ship)
        {
            return ship.Damage(board); // Assuming Damage is implemented for both Ship and ShipGroup
        }

        // This method destroys the ship (or group of ships)
        public static Board DestroyShip(Board board, IShipComponent ship)
        {
            var boardAfterShot = MakeShotsAroundShip(board, ship);
            return ship.Destroy(boardAfterShot); // Assuming Destroy is implemented for both Ship and ShipGroup
        }

        // Method that handles shooting around the ship (or ship group)
        private static Board MakeShotsAroundShip(Board board, IShipComponent ship)
        {
            if (ship is ShipGroup shipGroup)
            {
                // Handle shooting around each ship in the group
                foreach (var subShip in shipGroup.Ships)  // Ships is a public property of ShipGroup
                {
                    board = MakeShotsAroundCell(subShip, board); // Process each individual ship in the group
                }
            }
            else if (ship is Ship individualShip)
            {
                // Handle shooting around an individual ship
                board = MakeShotsAroundCell(individualShip, board);
            }

            return board;
        }

        // Method that shoots around the cells of a single ship
        private static Board MakeShotsAroundCell(IShipComponent ship, Board board)
        {
            if (ship is Ship individualShip)
            {
                // For each coordinate in the ship, make shots around it
                return individualShip.Coordinates.Aggregate(board, (currentBoard, targetCoords) => 
                    MakeShotsAroundCell(targetCoords, currentBoard));
            }

            return board;
        }

        // Method that shoots around a specific coordinate on the board
        private static Board MakeShotsAroundCell(Coordinates coords, Board board)
        {
            var newGrid = (Cell[,])board.CloneGrid().Clone();

            // Generate coordinates around the target coordinates to shoot at
            foreach (Coordinates coord in GenerateAdjacentCoordinates(coords))
            {
                if (board.CanShoot(coord))
                {
                    var cell = newGrid[coord.Row, coord.Col];
                    newGrid[coord.Row, coord.Col] = new Cell(Guid.NewGuid(), coord.Row, coord.Col, 
                        cell.IsIsland ? false : true, cell.IsShip, cell.IsIsland, cell.ImagePath);
                }
            }

            // Return the updated board with the new grid
            return BoardBuilder
                .From(board)
                .SetGrid(newGrid)
                .Build();
        }

        // Helper method to generate adjacent coordinates around a given coordinate
        private static IEnumerable<Coordinates> GenerateAdjacentCoordinates(Coordinates coords)
        {
            return Enumerable.Range(coords.Row - 1, 3)
                .SelectMany(row => Enumerable.Range(coords.Col - 1, 3)
                .Select(col => new Coordinates(Guid.NewGuid(), row, col)));
        }
    }
}
