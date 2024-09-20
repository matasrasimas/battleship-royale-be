
using battleship_royale_be.Data;
using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public class StartNewGameUseCase : IStartNewGameUseCase
    {
        private readonly BattleshipAPIContext _context;

        public StartNewGameUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public async Task<Guid> Start()
        {
            Board board = ShipsPlacer.PlaceShipsOnBoard();

            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < board.Grid.GetLength(0); i++) {
                for (int j = 0; j < board.Grid.GetLength(1); j++) {
                    cells.Add(board.Grid[i, j]);
                }
            }

            Game game = new Game(
                Guid.NewGuid(),
                cells,
                board.Ships,
                "",
                "IN_PROGRESS"
            );
 

            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();

            return game.Id;
        }
    }
}
