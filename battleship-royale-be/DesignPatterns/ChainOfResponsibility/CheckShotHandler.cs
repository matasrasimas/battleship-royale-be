using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.ChainOfResponsibility
{
    public class CheckShotHandler : Handler
    {
        public override List<Player> Handle(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount, Dictionary<Guid, int> shotsFired, Cell[,] grid, Board board, Ship targetShip)
        {
            if (!board.CanShoot(new Coordinates(Guid.NewGuid(), targetCoords.Row, targetCoords.Col)) || !attackerPlayer.IsYourTurn)
            {
                return new List<Player> {
                    PlayerBuilder.From(attackerPlayer).Build(),
                    PlayerBuilder.From(targetPlayer).Build()
                };
            }

            return HandleNext(attackerPlayer, targetPlayer, targetCoords, shotCount, shotsFired, grid, board, targetShip);
        }
    }
}
