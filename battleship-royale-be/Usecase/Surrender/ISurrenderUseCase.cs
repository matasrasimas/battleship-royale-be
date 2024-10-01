using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Surrender
{
    public interface ISurrenderUseCase
    {
        Task<Game?> Surrender(Guid gameId, string connectionId);
    }
}
