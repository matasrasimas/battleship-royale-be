using System.Collections.Generic;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Iterator
{
    public class ShipIterator : IIterator<Ship>
    {
        private readonly List<Ship> ships;
        private int currentIndex = -1;

        public ShipIterator(List<Ship> ships)
        {
            this.ships = ships;
        }

        public bool HasNext()
        {
            return currentIndex + 1 < ships.Count;
        }

        public Ship Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements in the list.");

            currentIndex++;
            return ships[currentIndex];
        }
    }
}
