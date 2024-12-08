using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;

namespace battleship_royale_be.DesignPatterns.Proxy.GameSurrenderer
{
    public interface GameSurrenderer
    {
        Game SurrenderGame(Game gameToSurrender, Player playerThatWantsToSurrender, Player playerThatDoesNotWantToSurrender);
    }
}
