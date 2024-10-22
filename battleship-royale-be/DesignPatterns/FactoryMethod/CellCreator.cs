using battleship_royale_be.DesignPatterns.FactoryMethod.Cells;

namespace battleship_royale_be.DesignPatterns.FactoryMethod
{
    public abstract class CellCreator
    {
        public void PlaceCellOnBoard(Cell[,] board, int row, int col) {
            Cell cellToPlace = CreateCell();
            board[row, col] = cellToPlace;
        }

        public abstract Cell CreateCell();
    }
}
