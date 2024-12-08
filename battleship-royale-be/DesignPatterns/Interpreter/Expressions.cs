using battleship_royale_be.Hubs;
using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Interpreter
{
    public class MessageExpression : IExpression
    {
        private string _message;
        public MessageExpression(string message)
        {
            _message = message;
        }
        public async Task Interpret(GameHub hub)
        {
            await hub.MessageCommand(_message);
        }
    }
    public class ShootExpression : IExpression
    {
        private ShotCoordinates _shotCoordinates;
        public ShootExpression(int row, int col)
        {
            _shotCoordinates = new ShotCoordinates(row, col);
        }
        public async Task Interpret(GameHub hub)
        {
            await hub.ShootCommand(_shotCoordinates);
        }
    }
    public class PauseExpression : IExpression
    {
        public async Task Interpret(GameHub hub)
        {
            await hub.PauseCommand();
        }
    }
    public class SurrenderExpression : IExpression
    {
        public async Task Interpret(GameHub hub)
        {
            await hub.HandleSurrender();
        }
    }
    public class UndoExpression : IExpression
    {
        public async Task Interpret(GameHub hub)
        {
            await hub.UndoCommand();
        }
    }
    public class InvalidExpression : IExpression
    {
        public async Task Interpret(GameHub hub)
        {
            await hub.InvalidCommand();
        }
    }
}