namespace battleship_royale_be.Usecase.FindGameUseCase
{
    public interface IFindGameUseCase
    {
        Task<Guid> FindGame();
    }
}
