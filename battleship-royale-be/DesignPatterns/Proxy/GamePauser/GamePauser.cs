using battleship_royale_be.Models;

namespace battleship_royale_be.DesignPatterns.Proxy.GamePauser
{
    public interface GamePauser
    {
        Game PauseGame(Game gameToPause, Player playerThatWantsToPause, Player playerThatDoesNotWantToPause);
    }
}
