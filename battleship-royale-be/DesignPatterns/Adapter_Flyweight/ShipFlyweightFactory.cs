using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Adapter_Flyweight
{
    public class ShipFlyweightFactory
    {
        private static Dictionary<string, IShip> shipMap = [];

        public static IShip GetShipFlyweight(string shipType)
        {
            if (shipMap.TryGetValue(shipType, out _))
            {
                Console.WriteLine("ShipFlyweightFactory: Reusing existing ship with type " + shipType + ".");
                return shipMap[shipType];
            }
            else
            {
                Console.WriteLine("ShipFlyweightFactory: Can't find a ship with type " + shipType + "." + " Creating a new one.");
                switch (shipType)
                {
                    case "Carrier":
                        shipMap.Add(shipType, new Carrier());
                        break;
                    case "Battleship":
                        shipMap.Add(shipType, new Battleship());
                        break;
                    case "Submarine":
                        shipMap.Add(shipType, new Submarine());
                        break;
                    case "Cruiser":
                        shipMap.Add(shipType, new Cruiser());
                        break;
                    case "Destroyer":
                        shipMap.Add(shipType, new Destroyer());
                        break;
                    default:
                        Console.WriteLine("ShipFlyweightFactory: Can't find a ship with type " + shipType + ".");
                        break;
                }
                return shipMap[shipType];
            }
        }
    }
}