using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Shoot
{
    public interface IShootUseCase
    {
        Task<Game?> Shoot(Guid id, ShotCoordinates shotCoords, string connectionId, int shotCount);

        Game GetBackup();
    }
}
