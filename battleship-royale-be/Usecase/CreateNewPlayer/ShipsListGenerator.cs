using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class ShipsListGenerator
    {
        public static List<Ship> Generate(int gameLevel)
        {
            List<ShipType> shipTypes = CreateShipTypesList(gameLevel);
            List<Ship> ships = ConvertShipTypesToShipObjects(shipTypes, gameLevel);

            Random rng = new Random();
            ships = ships.OrderBy(a => rng.Next()).ToList();

            return ships;
        }

        private static List<ShipType> CreateShipTypesList(int gameLevel)
        {
            return gameLevel == 1
                ? new List<ShipType>
                    {
                        ShipType.CARRIER, ShipType.CARRIER,
                        ShipType.BATTLESHIP, ShipType.BATTLESHIP,
                        ShipType.CRUISER,
                        ShipType.SUBMARINE,
                    }
                : new List<ShipType>
                    {
                        ShipType.CARRIER, ShipType.CARRIER, ShipType.CARRIER,
                        ShipType.BATTLESHIP, ShipType.BATTLESHIP, ShipType.BATTLESHIP,
                        ShipType.CRUISER, ShipType.CRUISER,
                        ShipType.SUBMARINE,
                        ShipType.DESTROYER,
                    };
        }

        private static List<Ship> ConvertShipTypesToShipObjects(List<ShipType> shipTypes, int gameLevel)
        {
            Random random = new Random();
            return shipTypes.Select(shipType =>
                ShipBuilder
                    .DefaultValues()
                    .SetHitPoints(GetShipHitPointsByType(shipType))
                    .SetIsHorizontal(random.NextDouble() < 0.5)
                    .SetCanMove(gameLevel == 2)
                    .Build()
            ).ToList();
        }

        private static int GetShipHitPointsByType(ShipType shipType)
        {
            return shipType switch
            {
                ShipType.CARRIER => 1,
                ShipType.BATTLESHIP => 2,
                ShipType.CRUISER => 3,
                ShipType.SUBMARINE => 4,
                ShipType.DESTROYER => 5,
                _ => throw new ArgumentOutOfRangeException(nameof(shipType), $"Not expected shipType value: {shipType}"),
            };
        }
    }

    public enum ShipType {
        CARRIER,
        BATTLESHIP,
        CRUISER,
        SUBMARINE,
        DESTROYER
    }
}
