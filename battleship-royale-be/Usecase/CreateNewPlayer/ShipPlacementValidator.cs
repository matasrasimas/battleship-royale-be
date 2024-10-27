using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class ShipPlacementValidator
    {
        public static bool CanPlaceShip(Cell[,] grid, Ship ship, Coordinates startCoords)
        {
            return IsShipInsideGameBoard(grid, ship, startCoords) && !IsOverlappingWithAdjacentShips(grid, ship, startCoords) && !IsOverlappingWithIslands(grid, ship, startCoords);
        }

        private static bool IsShipInsideGameBoard(Cell[,] grid, Ship ship, Coordinates startCoords)
        {
            Coordinates endCoords = ship.CalculateEndCoordinates(startCoords);
            return endCoords.Row < grid.GetLength(0) && endCoords.Col < grid.GetLength(1);
        }

        private static bool IsOverlappingWithAdjacentShips(Cell[,] grid, Ship ship, Coordinates startCoords)
        {
            Coordinates endCoords = ship.CalculateEndCoordinates(startCoords);

            for (int i = startCoords.Row - 1; i <= endCoords.Row + 1; i++)
            {
                for (int j = startCoords.Col - 1; j <= endCoords.Col + 1; j++)
                {
                    Coordinates coordsToCheck = new Coordinates(Guid.NewGuid(), i, j);
                    if (IsCellInsideGrid(grid, coordsToCheck) && CellContainsShip(grid, coordsToCheck))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsOverlappingWithIslands(Cell[,] grid, Ship ship, Coordinates startCoords)
        {
            Coordinates endCoords = ship.CalculateEndCoordinates(startCoords);

            for (int i = startCoords.Row; i <= endCoords.Row; i++)
            {
                for (int j = startCoords.Col; j <= endCoords.Col; j++)
                {
                    Coordinates coordsToCheck = new Coordinates(Guid.NewGuid(), i, j);
                    if (IsCellInsideGrid(grid, coordsToCheck) && CellContainsIsland(grid, coordsToCheck))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsCellInsideGrid(Cell[,] grid, Coordinates coords)
        {
            return coords.Row >= 0 && coords.Row < grid.GetLength(0) &&
                   coords.Col >= 0 && coords.Col < grid.GetLength(1);
        }

        private static bool CellContainsShip(Cell[,] grid, Coordinates coords)
        {
            return grid[coords.Row, coords.Col].IsShip;
        }

        private static bool CellContainsIsland(Cell[,] grid, Coordinates coords) {
            return grid[coords.Row, coords.Col].IsIsland;
        }
    }
}
