using battleship_royale_be.DesignPatterns.FactoryMethod;
using battleship_royale_be.DesignPatterns.FactoryMethod.Cells;
using System;

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
            var random = new Random();
            var islandCells = new HashSet<(int, int)>();

            CellCreator waterCellCreator = new WaterCellCreator();
            CellCreator islandCellCreator = new IslandCellCreator();

            DesignPatterns.FactoryMethod.Cells.Cell[,] builtGrid = new DesignPatterns.FactoryMethod.Cells.Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    waterCellCreator.PlaceCellOnBoard(builtGrid, i, j);
                }
            }

            while (islandCells.Count < 10)
            {
                int randomRow = random.Next(0, 10);
                int randomCol = random.Next(0, 10);

                if (!islandCells.Contains((randomRow, randomCol)))
                {
                    islandCells.Add((randomRow, randomCol));
                    islandCellCreator.PlaceCellOnBoard(builtGrid, randomRow, randomCol);
                }
            }


            builder.grid = new Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    DesignPatterns.FactoryMethod.Cells.Cell cellToPlace = builtGrid[i, j];
                    if (cellToPlace is WaterCell waterCell)
                        builder.grid[i, j] = new Cell(waterCell.Id, i, j, waterCell.IsHit , waterCell.IsShip, false);

                    else if(cellToPlace is IslandCell islandCell)
                        builder.grid[i, j] = new Cell(islandCell.Id, i, j, false, false, true);

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
