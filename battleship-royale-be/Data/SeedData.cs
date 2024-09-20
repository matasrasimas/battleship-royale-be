using battleship_royale_be.Models;

namespace battleship_royale_be.Data
{
    //public static class SeedData
    //{
    //    public static async Task Seed(BattleshipAPIContext dbContext) {
    //        dbContext.Database.EnsureDeleted();
    //        dbContext.Database.EnsureCreated();

    //        List<Cell> board = new List<Cell>();
    //        board.Add(new Cell
    //        {
    //            Row = 1,
    //            Col = 1,
    //            IsHit = true,
    //            IsShip = false,
    //        });

    //        List<Coordinates> shipCoordinates = new List<Coordinates>();
    //        shipCoordinates.Add(new Coordinates { Row = 5, Col = 4 });

    //        List<Ship> ships = new List<Ship>();
    //        ships.Add(new Ship
    //        {
    //            Id = Guid.NewGuid(),
    //            HitPoints = 5,
    //            IsHorizontal = true,
    //            Coordinates = shipCoordinates,

    //        });

    //        Game game = new Game {
    //            Id = Guid.NewGuid(),
    //            Board = board,
    //            Ships = ships,
    //            ShotResultMessage = "lmao",
    //            Status = "IN_PROGRESS"
    //        };

    //        await dbContext.Games.AddAsync(game);
    //        await dbContext.SaveChangesAsync();
    //    }
    //}
}
