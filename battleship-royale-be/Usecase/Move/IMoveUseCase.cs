using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Move
{
    public interface IMoveUseCase
    {
        Task<Game?> Move(Guid id, string connectionId, int hitPoints);
        Game GetBackup();
    }
}
