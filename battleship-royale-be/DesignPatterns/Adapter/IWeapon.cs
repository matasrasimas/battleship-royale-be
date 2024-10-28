using battleship_royale_be.Models;
namespace battleship_royale_be.DesignPatterns.Adapter
{
    public interface IWeapon
    {
        int GetDamage(Ship ship);
    }
}
