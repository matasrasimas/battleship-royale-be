namespace battleship_royale_be.Models.Converters
{
    public static class GridConverter
    {
        public static List<Cell> FromArrayToList(Cell[,] input) {
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < input.GetLength(0); i++) {
                for (int j = 0; j < input.GetLength(1); j++) {
                    cells.Add(new Cell(Guid.NewGuid(), i, j, input[i, j].IsHit, input[i, j].IsShip, input[i, j].IsIsland));
                }
            }
            return cells;
        }

        public static Cell[,] FromListToArray(List<Cell> input) {
            Cell[,] grid = new Cell[10, 10];
            List<Cell> sortedCells = input
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();
            foreach (Cell cell in sortedCells)
            {
                int row = cell.Row;
                int col = cell.Col;
                grid[row, col] = new Cell(Guid.NewGuid(), row, col, cell.IsHit, cell.IsShip, cell.IsIsland);
            }
            return grid;
        }
    }
}
