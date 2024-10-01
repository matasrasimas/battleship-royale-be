using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.CreateNewGame
{
    public interface IAddPlayerToGameUseCase
    {
        void AddPlayer(Guid gameId, Player player);
    }
}
