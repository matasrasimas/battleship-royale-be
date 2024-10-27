using System;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class TripleShotStrategy : IShootingStrategy
    {
        public void ExecuteShoot(ShotCoordinates coordinates)
        {
            Console.WriteLine($"Triple shot fired at coordinates ({coordinates.Row}, {coordinates.Col})");
            // Implement triple-shot logic here
        }
    }
}
