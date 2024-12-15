namespace battleship_royale_be.DesignPatterns.Iterator
{
    using battleship_royale_be.Models;

    public class CoordinatesIterator : IIterator<Coordinates>
    {
        private readonly List<Coordinates> coordinates;
        private int currentIndex = -1;

        public CoordinatesIterator(List<Coordinates> coordinates)
        {
            this.coordinates = coordinates;
        }

        public bool HasNext()
        {
            return currentIndex + 1 < coordinates.Count;
        }

        public Coordinates Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements in the list.");
            currentIndex++;
            return coordinates[currentIndex];
        }
    }
}
