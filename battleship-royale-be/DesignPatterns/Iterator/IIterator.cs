namespace battleship_royale_be.DesignPatterns.Iterator
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
