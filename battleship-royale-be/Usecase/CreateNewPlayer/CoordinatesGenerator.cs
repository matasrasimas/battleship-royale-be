using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public static class CoordinatesGenerator
    {
        public static Coordinates GenerateCoordinatesRandomly(int limit)
        {
            Random random = new Random();
            int randomRow = random.Next(limit);
            int randomCol = random.Next(limit);
            return new Coordinates(Guid.NewGuid(), randomRow, randomCol);
        }
    }
}
