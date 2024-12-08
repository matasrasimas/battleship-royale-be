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
        public void Interpret(GameHub hub)
        {
            hub.MessageCommand(_message);
        }
    }
    public class ShootExpression : IExpression
    {
        private ShotCoordinates _shotCoordinates;
        public ShootExpression(int row, int col)
        {
            _shotCoordinates = new ShotCoordinates(row, col);
        }
        public void Interpret(GameHub hub)
        {
            hub.ShootCommand(_shotCoordinates);
        }
    }
    public class PauseExpression : IExpression
    {
        public void Interpret(GameHub hub)
        {
            hub.PauseCommand();
        }
    }
    public class SurrenderExpression : IExpression
    {
        public async void Interpret(GameHub hub)
        {
            await hub.HandleSurrender();
        }
    }
    public class UndoExpression : IExpression
    {
        public void Interpret(GameHub hub)
        {
            hub.UndoCommand();
        }
    }
    public class InvalidExpression : IExpression
    {
        public void Interpret(GameHub hub)
        {
            hub.InvalidCommand();
        }
    }
}