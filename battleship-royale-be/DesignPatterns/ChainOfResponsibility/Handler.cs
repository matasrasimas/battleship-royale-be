using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.ChainOfResponsibility
{
    public abstract class Handler
    {
        private Handler next;

        public Handler SetNextHandler(Handler next) {
            this.next = next;
            return this;
        }

        protected List<Player> HandleNext(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount, Dictionary<Guid, int> shotsFired, Cell[,] grid, Board board, Ship targetShip) {
            if (next == null)
                return new List<Player> {
                    PlayerBuilder.From(attackerPlayer).Build(),
                    PlayerBuilder.From(targetPlayer).Build(),
                };

            return next.Handle(attackerPlayer, targetPlayer, targetCoords, shotCount, shotsFired, grid, board, targetShip);
        }

        public abstract List<Player> Handle(Player attackerPlayer, Player targetPlayer, ShotCoordinates targetCoords, int shotCount, Dictionary<Guid, int> shotsFired, Cell[,] grid, Board board, Ship targetShip);
    }
}
