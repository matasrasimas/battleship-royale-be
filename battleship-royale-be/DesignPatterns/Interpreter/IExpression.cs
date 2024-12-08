using battleship_royale_be.Hubs;

namespace battleship_royale_be.DesignPatterns.Interpreter
{
    public interface IExpression
    {
        void Interpret(GameHub hub);
    }
}