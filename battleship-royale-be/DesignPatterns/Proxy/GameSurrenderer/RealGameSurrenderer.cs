using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.Proxy.GameSurrenderer
{
    public class RealGameSurrenderer : GameSurrenderer
    {
        public Game SurrenderGame(Game gameToSurrender, Player playerThatWantsToSurrender, Player playerThatDoesNotWantToSurrender)
        {
            List<Player> playersListAfterSurrender = new List<Player> {
                PlayerBuilder
                  .From(playerThatWantsToSurrender)
                  .SetGameStatus("LOST")
                  .Build(),

                PlayerBuilder
                  .From(playerThatDoesNotWantToSurrender)
                  .SetGameStatus("WON")
                  .Build()
            };

            return GameBuilder.From(gameToSurrender).SetPlayers(playersListAfterSurrender).Build();
        }
    }
}
