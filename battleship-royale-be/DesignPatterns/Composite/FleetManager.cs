using System.Collections.Generic;
using System.Linq;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Composite
{
    public static class FleetManager
    {
        public static void MoveShipsBySize(List<IShipComponent> components, int size)
        {
            Console.WriteLine($"Moving all ships of size {size}:");
            foreach (var component in components)
            {
                if (component is Ship ship && ship.HitPoints == size)
                {
                    //Todo move
                }
                else if (component is Fleet fleet)
                {
                    MoveShipsBySize(fleet.GetComponents(), size);
                }
            }
        }
    }
}
