using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.DesignPatterns.Adapter;
using System.Collections.Generic;
using System.Linq;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class ShipsListGenerator
    {
        public static List<Ship> Generate(int gameLevel)
        {
            List<IShip> shipTypes = CreateShipTypesList(gameLevel);
            List<Ship> ships = ConvertShipTypesToShipObjects(shipTypes, gameLevel);

            Random rng = new Random();
            ships = ships.OrderBy(a => rng.Next()).ToList();

            return ships;
        }

        private static List<IShip> CreateShipTypesList(int gameLevel)
        {
            var shipTypes = new List<IShip>();

            if (gameLevel == 1)
            {
                shipTypes.Add(new Carrier());
                shipTypes.Add(new Carrier());
                shipTypes.Add(new Battleship());
                shipTypes.Add(new Battleship());
                shipTypes.Add(new Cruiser());
                shipTypes.Add(new Submarine());
            }
            else
            {
                shipTypes.Add(new Carrier());
                shipTypes.Add(new Carrier());
                shipTypes.Add(new Carrier());
                shipTypes.Add(new Battleship());
                shipTypes.Add(new Battleship());
                shipTypes.Add(new Battleship());
                shipTypes.Add(new Cruiser());
                shipTypes.Add(new Cruiser());
                shipTypes.Add(new Submarine());
                shipTypes.Add(new Destroyer());
            }

            return shipTypes;
        }

        private static List<Ship> ConvertShipTypesToShipObjects(List<IShip> shipTypes, int gameLevel)
        {
            Random random = new Random();
            return shipTypes.Select(shipType =>
            {
                ShipAdapter shipAdapter = new ShipAdapter(shipType);
                Ship ship = ShipBuilder
                    .DefaultValues()
                    .SetHitPoints(shipAdapter.GetHitPoints())
                    .SetIsHorizontal(random.NextDouble() < 0.5)
                    .SetCanMove(gameLevel == 2)
                    .Build();

                return ship;
            }).ToList();
        }
    }
}
