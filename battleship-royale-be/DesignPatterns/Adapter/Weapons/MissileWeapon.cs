namespace battleship_royale_be.DesignPatterns.Adapter
{
    public class MissileWeapon : IWeapon
    {
        public void Fire(int targetX, int targetY)
        {
            Console.WriteLine($"Launching missile at coordinates ({targetX}, {targetY})");
        }
    }
}