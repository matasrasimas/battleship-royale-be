namespace battleship_royale_be.Usecase.StartNewGame
{
    public interface IStartNewGameUseCase
    {
        Task<Guid> Start();
    }
}
