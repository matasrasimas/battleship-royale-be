using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Memento
{
    public interface IMemento
    {
        Game GetState();
    }
}
