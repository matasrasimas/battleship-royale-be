using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.DesignPatterns.ChainOfResponsibility;

namespace battleship_royale_be.Usecase.Shoot
{
    public static class ShotHandler
    {
        private static Dictionary<Guid, int> shotsFired = new Dictionary<Guid, int>();

        public static List<Player> HandleShot(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount)
        {
            Cell[,] grid = GridConverter.FromListToArray(targetPlayer.Cells);
            Board board = new Board(
                (Cell[,])grid.Clone(),
                new List<Ship>(targetPlayer.Ships)
            );
            Ship? targetShip = board.FindShipByCoordinates(targetCoords);

            if (!shotsFired.ContainsKey(attackerPlayer.Id))
            {
                shotsFired[attackerPlayer.Id] = 0;
            }

            Handler handler = new CheckShotHandler()
                .SetNextHandler(new PerformShotHandler()
                .SetNextHandler(new DamageShipHandler()
                .SetNextHandler(new DestroyShipHandler())));

            return handler.Handle(attackerPlayer, targetPlayer, targetCoords, shotCount, shotsFired, grid, board, targetShip);
        }     
    }
}
