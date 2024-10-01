using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.StartNewGame

{
    public interface ICreateNewPlayerUseCase
    {
        Player CreatePlayer(string connectionId);
    }
}
