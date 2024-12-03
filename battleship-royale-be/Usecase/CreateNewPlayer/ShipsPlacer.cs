using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class ShipsPlacer
    {

        public static Board PlaceShipsOnBoard(int gameLevel)
        {
            var board = gameLevel == 1
                ? BoardBuilder.BuildLevel1Board()
                : BoardBuilder.BuildLevel2Board();

            int shipsLimit = gameLevel == 2 ? 10 : 6;

            List<Ship> ships = ShipsListGenerator.Generate(gameLevel);
            foreach (Ship ship in ships)
            {
                board = PlaceShipRandomly(board, ship, gameLevel, shipsLimit);
            }
            return board;
        }

        private static Board PlaceShipRandomly(Board board, Ship ship, int gameLevel, int shipsLimit)
        {
            if (board.Ships.Count == shipsLimit)
                return board;

            int maxAttempts = 1000;
            return AttemptToPlaceShip(board, ship, maxAttempts, gameLevel);
        }

        private static Board AttemptToPlaceShip(Board board, Ship ship, int maxAttempts, int gameLevel)
        {
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                Coordinates randomCoordinates = CoordinatesGenerator.GenerateCoordinatesRandomly(gameLevel == 2 ? 15 : 10);
                if (ShipPlacementValidator.CanPlaceShip(board.Grid, ship, randomCoordinates))
                    return PlaceShip(board, ship, randomCoordinates);
                attempts++;
            }
            return PlaceShipsOnBoard(gameLevel);
        }

        private static Board PlaceShip(Board board, Ship ship, Coordinates startCoords)
        {
            return ship.IsHorizontal
                ? PlaceShipHorizontally(board, ship, startCoords)
                : PlaceShipVertically(board, ship, startCoords);
        }

        private static Board PlaceShipVertically(Board board, Ship ship, Coordinates startCoords)
        {
            int startRow = startCoords.Row;
            int startCol = startCoords.Col;
            List<Coordinates> newCoordinates = new List<Coordinates>();
            var newGrid = (Cell[,])board.CloneGrid();

            for (int i = 0; i < ship.HitPoints; i++)
            {
                newCoordinates.Add(new Coordinates(Guid.NewGuid(), startRow + i, startCol));
                // add image path
                newGrid[startRow + i, startCol] = new Cell(Guid.NewGuid(), startRow + i, startCol, false, true, false, ship.ImagePath);
            }

            return AddShipToGame(board, ship, newCoordinates, newGrid);
        }

        private static Board PlaceShipHorizontally(Board board, Ship ship, Coordinates startCoords)
        {
            int startRow = startCoords.Row;
            int startCol = startCoords.Col;
            List<Coordinates> newCoordinates = new List<Coordinates>();
            var newGrid = (Cell[,])board.CloneGrid();

            for (int i = 0; i < ship.HitPoints; i++)
            {
                newCoordinates.Add(new Coordinates(Guid.NewGuid(), startRow, startCol + i));
                newGrid[startRow, startCol + i] = new Cell(Guid.NewGuid(), startRow, startCol + i, false, true, false, ship.ImagePath);
            }

            return AddShipToGame(board, ship, newCoordinates, newGrid);
        }

        private static Board AddShipToGame(Board board, Ship ship, List<Coordinates> newCoordinates, Cell[,] newGrid)
        {
            Ship newShip = ShipBuilder
                .From(ship)
                .SetCoordinates(newCoordinates)
                .Build();

            List<Ship> newShipsList = new List<Ship>(board.Ships)
            {
              newShip
            };

            return BoardBuilder
                .From(board)
                .SetGrid(newGrid)
                .SetShips(newShipsList)
                .Build();
        }
    }
}
