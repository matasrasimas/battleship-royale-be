
using battleship_royale_be.Data;
using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.StartNewGame
{
    public class CreateNewPlayerUseCase : ICreateNewPlayerUseCase
    {

        public CreateNewPlayerUseCase()
        {
        }

        public Player CreatePlayer(string connectionId, int gameLevel)
        {
            Board board = ShipsPlacer.PlaceShipsOnBoard(gameLevel);

            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < board.Grid.GetLength(0); i++) {
                for (int j = 0; j < board.Grid.GetLength(1); j++) {
                    cells.Add(board.Grid[i, j]);
                }
            }

            Player player = new Player(
                Guid.NewGuid(),
                connectionId,
                cells,
                board.Ships,
                "IN_PROGRESS",
                false,
                0
            );

            return player;
        }
    }
}
