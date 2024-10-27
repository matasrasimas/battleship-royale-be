using battleship_royale_be.DesignPatterns.Adapter;

public class HeavyCannon : IWeapon
{
    public void Fire(int targetX, int targetY)
    {
        Console.WriteLine($"Firing heavy cannon at coordinates ({targetX}, {targetY})");
    }
}