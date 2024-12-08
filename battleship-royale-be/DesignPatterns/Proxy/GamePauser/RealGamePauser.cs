using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.Proxy
{
    public class RealGamePauser : GamePauser
    {
        public Game PauseGame(Game gameToPause, Player playerThatWantsToPause, Player playerThatDoesNotWantToPause)
        {
            List<Player> playersListAfterPause =
            [
                PlayerBuilder
                  .From(playerThatWantsToPause)
                  .SetGameStatus("PAUSED_HOST")
                  .Build(),

                PlayerBuilder
                  .From(playerThatDoesNotWantToPause)
                  .SetGameStatus("PAUSED")
                  .Build()
            ];

            return GameBuilder.From(gameToPause).SetPlayers(playersListAfterPause).Build();
        }
    }
}
