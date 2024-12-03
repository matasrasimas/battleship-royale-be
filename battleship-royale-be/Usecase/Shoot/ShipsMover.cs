using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.Usecase.StartNewGame;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShipsMover
    {
        public static Player MoveShips(Player player, List<Ship> ships, Cell[,] grid) {
            List<Ship> updatedShips = new List<Ship>();
            Cell[,] updatedGrid = (Cell[,])grid.Clone();
            Random random = new Random();
            int direction = random.Next(1, 5);

            foreach (Ship ship in ships) {
                if (ShipIsDamaged(ship, updatedGrid) || !ship.CanMove) {
                    updatedShips.Add(ShipBuilder.From(ship).Build());
                    continue;
                }
                List<Coordinates> newCoordinates = new List<Coordinates>();
                foreach (Coordinates coords in ship.Coordinates) {
                    Coordinates newCoord = CreateCoordByDirection(coords, direction);
                    newCoordinates.Add(newCoord);
                    updatedGrid[coords.Row, coords.Col].IsShip = false;
                }
                Ship shipWithUpdatedCoords = ShipBuilder.From(ship).SetCoordinates(newCoordinates).Build();
                if (ShipPlacementValidator.CanPlaceShip(updatedGrid, shipWithUpdatedCoords, shipWithUpdatedCoords.calculateStartCoordinates()))
                {
                    foreach (Coordinates newCoord in newCoordinates)
                    {
                        updatedGrid[newCoord.Row, newCoord.Col].IsShip = true;
                        updatedGrid[newCoord.Row, newCoord.Col].ImagePath = ship.ImagePath;
                    }
                    updatedShips.Add(shipWithUpdatedCoords);
                }
                else {
                    foreach (Coordinates coord in ship.Coordinates) {
                        updatedGrid[coord.Row, coord.Col].IsShip = true;
                    }
                    updatedShips.Add(ShipBuilder.From(ship).Build());
                }
            }

            return PlayerBuilder
                .From(player)
                .SetShips(updatedShips)
                .SetCells(GridConverter.FromArrayToList(updatedGrid))
                .Build();
        }

        private static Coordinates CreateCoordByDirection(Coordinates coords, int direction) {
            return direction switch
            {
                1 => new Coordinates(coords.Id, coords.Row + 1, coords.Col),
                2 => new Coordinates(coords.Id, coords.Row - 1, coords.Col),
                3 => new Coordinates(coords.Id, coords.Row, coords.Col + 1),
                4 => new Coordinates(coords.Id, coords.Row, coords.Col - 1),
                _ => new Coordinates(coords.Id, coords.Row + 1, coords.Col),
            };
        }

        private static bool ShipIsDamaged(Ship ship, Cell[,] grid) {
            foreach (Coordinates coords in ship.Coordinates) {
                if (grid[coords.Row, coords.Col].IsHit) return true;
            }
            return false;
        }
    }
}
