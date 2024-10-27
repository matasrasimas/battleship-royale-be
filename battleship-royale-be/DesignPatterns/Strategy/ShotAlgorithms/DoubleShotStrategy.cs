using System;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public class DoubleShotStrategy : IShootingStrategy
    {
        public void ExecuteShoot(ShotCoordinates coordinates)
        {
            Console.WriteLine($"Double shot fired at coordinates ({coordinates.Row}, {coordinates.Col})");
            // Implement double-shot logic here
        }
    }
}
