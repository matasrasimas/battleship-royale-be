using battleship_royale_be.Models;
using System;

namespace battleship_royale_be.DesignPatterns.Iterator
{
    public class GridIterator
    {
        private Cell[,] grid;
        private int currentRow;
        private int currentCol;

        public GridIterator(Cell[,] grid)
        {
            this.grid = grid;
            currentRow = 0;
            currentCol = 0;
        }

        public bool HasNext()
        {
            return currentRow < grid.GetLength(0) && currentCol < grid.GetLength(1);
        }

        public Cell Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements in the grid.");

            Cell currentCell = grid[currentRow, currentCol];

            currentCol++;
            if (currentCol >= grid.GetLength(1))
            {
                currentCol = 0;
                currentRow++;
            }

            return currentCell;
        }

        public (int, int) NextPosition()
        {
            return (currentRow, currentCol);
        }
    }
}
