using battleship_royale_be.DesignPatterns.Iterator;

namespace battleship_royale_be.Models.Builders
{
    public class PlayerBuilder
    {
        private Guid id;
        private string connectionId;
        private List<Cell> cells;
        private List<Ship> ships;
        private string gameStatus;
        private bool isYourTurn;
        private int points;

        private PlayerBuilder()
        {
        }

        public static PlayerBuilder From(Player player)
        {
            var grid = CreateGridFromCells(player.Cells);
            var gridIterator = new GridIterator(grid);
            List<Cell> clonedCells = new List<Cell>();
            while (gridIterator.HasNext())
            {
                var cell = gridIterator.Next();
                clonedCells.Add(new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip, cell.IsIsland, cell.ImagePath));
            }
            List<Cell> sortedCells = clonedCells
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();
            var shipIterator = new ShipIterator(player.Ships);
            List<Ship> clonedShips = new List<Ship>();
            while (shipIterator.HasNext())
            {
                Ship ship = shipIterator.Next();
                var clonedCoordinates = ship.Coordinates.Select(coords => new Coordinates(Guid.NewGuid(), coords.Row, coords.Col)).ToList();
                clonedShips.Add(new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, ship.CanMove, clonedCoordinates, ship.ImagePath));
            }

            return new PlayerBuilder
            {
                id = player.Id,
                connectionId = player.ConnectionId,
                cells = sortedCells,
                ships = clonedShips,
                gameStatus = player.GameStatus,
                isYourTurn = player.IsYourTurn,
                points = player.Points,
            };
        }

        public static PlayerBuilder DefaultValues()
        {
            return new PlayerBuilder
            {
                id = Guid.NewGuid(),
                connectionId = string.Empty,
                cells = new List<Cell>(),
                ships = new List<Ship>(),
                gameStatus = "IN_PROGRESS",
                isYourTurn = false,
                points = 0,
            };
        }

        public PlayerBuilder SetId(Guid id)
        {
            this.id = id;
            return this;
        }

        public PlayerBuilder SetConnectionId(string connectionId)
        {
            this.connectionId = connectionId;
            return this;
        }

        public PlayerBuilder SetCells(List<Cell> cells)
        {
            var grid = CreateGridFromCells(cells);
            var gridIterator = new GridIterator(grid);
            List<Cell> clonedCells = new List<Cell>();
            while (gridIterator.HasNext())
            {
                var cell = gridIterator.Next();
                clonedCells.Add(new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip, cell.IsIsland, cell.ImagePath));
            }
            this.cells = clonedCells.OrderBy(c => c.Row).ThenBy(c => c.Col).ToList();
            return this;
        }

        public PlayerBuilder SetShips(List<Ship> ships)
        {
            var shipIterator = new ShipIterator(ships);
            List<Ship> clonedShips = new List<Ship>();
            while (shipIterator.HasNext())
            {
                Ship ship = shipIterator.Next();
                var clonedCoordinates = ship.Coordinates.Select(coords => new Coordinates(Guid.NewGuid(), coords.Row, coords.Col)).ToList();
                clonedShips.Add(new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, ship.CanMove, clonedCoordinates, ship.ImagePath));
            }
            this.ships = clonedShips;
            return this;
        }

        public PlayerBuilder SetGameStatus(string gameStatus)
        {
            this.gameStatus = gameStatus;
            return this;
        }

        public PlayerBuilder SetIsYourTurn(bool isYourTurn)
        {
            this.isYourTurn = isYourTurn;
            return this;
        }

        public PlayerBuilder SetPoints(int points)
        {
            this.points = points;
            return this;
        }

        public Player Build()
        {
            return new Player(id, connectionId, cells, ships, gameStatus, isYourTurn, points);
        }

        private static Cell[,] CreateGridFromCells(List<Cell> cells)
        {
            int gridSize = 10;
            var grid = new Cell[gridSize, gridSize];
            foreach (var cell in cells)
            {
                grid[cell.Row, cell.Col] = cell;
            }
            return grid;
        }
    }
}
