namespace battleship_royale_be.Models.Builders
{
    public class BoardBuilder
    {
        private Cell[,] grid;
        private List<Ship> ships;

        private BoardBuilder() { }

        public static BoardBuilder From(Board board)
        {
            var builder = new BoardBuilder();

            builder.grid = (Cell[,])board.Grid.Clone();

            builder.ships = new List<Ship>(board.Ships);

            return builder;
        }

        public static BoardBuilder DefaultValues()
        {
            var builder = new BoardBuilder();

            builder.grid = new Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    builder.grid[i, j] = new Cell(Guid.NewGuid(), i, j, false, false);
                }
            }

            builder.ships = new List<Ship>();

            return builder;
        }

        public BoardBuilder SetGrid(Cell[,] grid)
        {
            this.grid = (Cell[,])grid.Clone();
            return this;
        }

        public BoardBuilder SetShips(List<Ship> ships)
        {
            this.ships = new List<Ship>(ships);
            return this;
        }

        public Board Build()
        {
            return new Board(grid, ships);
        }
    }
}
