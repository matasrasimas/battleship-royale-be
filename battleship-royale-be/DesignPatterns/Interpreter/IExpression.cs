using battleship_royale_be.Hubs;

namespace battleship_royale_be.DesignPatterns.Interpreter
{
    public interface IExpression
    {
        Task Interpret(GameHub hub);
    }
}