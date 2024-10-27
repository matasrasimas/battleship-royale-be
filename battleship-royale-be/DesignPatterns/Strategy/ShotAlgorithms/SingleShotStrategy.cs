using System;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class SingleShotStrategy : IShootingStrategy
    {
        public void ExecuteShoot(ShotCoordinates coordinates)
        {
            Console.WriteLine($"Single shot fired at coordinates ({coordinates.Row}, {coordinates.Col})");
            // Implement single-shot logic here
        }
    }
}
