using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.GetGameById
{
    public interface IGetGameByIdUseCase
    {
        Task<Game?> Get(Guid id);
    }
}
