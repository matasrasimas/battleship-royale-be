using System.Text.RegularExpressions;

namespace battleship_royale_be.DesignPatterns.Interpreter
{
    public static partial class CommandParser
    {
        public static IExpression Parse(string input)
        {
            return input switch
            {
                var str when !str.StartsWith('/') => new MessageExpression(input),
                "/surrender" => new SurrenderExpression(),
                "/pause" => new PauseExpression(),
                "/undo" => new UndoExpression(),
                var str when MyRegex().IsMatch(str) => new ShootExpression(
                                                    int.Parse(input.Split(' ')[1]) - 1,
                                                    int.Parse(input.Split(' ')[2]) - 1),
                _ => new InvalidExpression(),
            };
        }
        [GeneratedRegex(@"^/shoot \d+ \d+$")]
        private static partial Regex MyRegex();
    }
}