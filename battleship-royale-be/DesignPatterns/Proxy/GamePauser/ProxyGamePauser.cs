using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.Proxy.GamePauser
{
    public class ProxyGamePauser : GamePauser
    {
        private GamePauser gamePauser = new RealGamePauser();

        public Game PauseGame(Game gameToPause, Player playerThatWantsToPause, Player playerThatDoesNotWantToPause)
        {
            if (!playerThatWantsToPause.IsYourTurn)
                return GameBuilder.From(gameToPause).Build();
            return gamePauser.PauseGame(gameToPause, playerThatWantsToPause, playerThatDoesNotWantToPause);
        }
    }
}
