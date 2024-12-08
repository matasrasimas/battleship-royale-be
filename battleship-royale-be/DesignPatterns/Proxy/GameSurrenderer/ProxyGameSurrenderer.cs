using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.Proxy.GameSurrenderer
{
    public class ProxyGameSurrenderer : GameSurrenderer
    {
        private GameSurrenderer gameSurrenderer = new RealGameSurrenderer();

        public Game SurrenderGame(Game gameToSurrender, Player playerThatWantsToSurrender, Player playerThatDoesNotWantToSurrender)
        {
            if (!playerThatWantsToSurrender.IsYourTurn)
                return GameBuilder.From(gameToSurrender).Build();
            return gameSurrenderer.SurrenderGame(gameToSurrender, playerThatWantsToSurrender, playerThatDoesNotWantToSurrender);
        }
    }
}
