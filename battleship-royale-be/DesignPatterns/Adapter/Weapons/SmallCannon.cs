namespace battleship_royale_be.DesignPatterns.Adapter
{
    public class SmallCannon : IWeapon
    {
        public void Fire(int targetX, int targetY)
        {
            Console.WriteLine($"Firing small cannon at coordinates ({targetX}, {targetY})");
        }
    }
}