using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Strategy
{
    public interface IShotStrategy
    {
        int GetMaxShots(Ship ship);
    }
}