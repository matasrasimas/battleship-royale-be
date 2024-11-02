using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Pause
{
    public interface IPauseUseCase
    {
        Task<Game?> Pause(Guid gameId, string connectionId);
        Game GetBackup();
    }
}
