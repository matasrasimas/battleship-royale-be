﻿using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.DesignPatterns.Adapter_Flyweight;
using System.Collections.Generic;
using System.Linq;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class ShipsListGenerator
    {
        // change this method to use flyweight pattern
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
        // flyweight pattern here
        // concept : 
        // flyweight = shipFlyweightFactory.GetShipFlyweight(shipType);
        // Ship ship = ShipBuilder.DefaultValues().SetImagePath(flyweight.GetImagePath()).SetHitPoints(flyweight.GetHitPoints()).SetIsHorizontal(random.NextDouble() < 0.5).SetCanMove(gameLevel == 2).Build();
        private static List<Ship> ConvertShipTypesToShipObjects(List<IShip> shipTypes, int gameLevel)
        {
            Random random = new Random();
            return shipTypes.Select(shipType =>
            {
                IShip shipFlyweight = ShipFlyweightFactory.GetShipFlyweight(shipType.GetName());
                ShipAdapter shipAdapter = new ShipAdapter(shipType);
                Ship ship = ShipBuilder
                    .DefaultValues()
                    .SetHitPoints(shipAdapter.GetHitPoints())
                    .SetIsHorizontal(random.NextDouble() < 0.5)
                    .SetCanMove(gameLevel == 2)
                    .SetImagePath(shipFlyweight.GetImagePath())
                    .Build();

                return ship;
            }).ToList();
        }
    }
}
