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
        private int shotsRemaining;

        private PlayerBuilder()
        {
        }

        public static PlayerBuilder From(Player player)
        {
            // Clone cells with new Guid for each one and sort
            List<Cell> clonedCells = player.Cells
                .Select(cell => new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip, cell.IsIsland, cell.ImagePath))
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();

            // Clone ships and coordinates
            List<Ship> clonedShips = player.Ships.Select(ship =>
            {
                var clonedCoordinates = ship.Coordinates.Select(coords => new Coordinates(Guid.NewGuid(), coords.Row, coords.Col)).ToList();
                return new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, ship.CanMove, clonedCoordinates, ship.ImagePath);
            }).ToList();

            return new PlayerBuilder
            {
                id = player.Id,
                connectionId = player.ConnectionId,
                cells = clonedCells,
                ships = clonedShips,
                gameStatus = player.GameStatus,
                isYourTurn = player.IsYourTurn,
                points = player.Points,
                shotsRemaining = player.ShotsRemaining,
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
                shotsRemaining = 1,
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
            // Clone cells with new Guid for each one and sort
            this.cells = cells
                .Select(cell => new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip, cell.IsIsland, cell.ImagePath))
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();
            return this;
        }

        public PlayerBuilder SetShips(List<Ship> ships)
        {
            // Clone ships and coordinates with new Guid for each coordinate
            this.ships = ships.Select(ship =>
            {
                var clonedCoordinates = ship.Coordinates.Select(coords => new Coordinates(Guid.NewGuid(), coords.Row, coords.Col)).ToList();
                return new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, ship.CanMove, clonedCoordinates, ship.ImagePath);
            }).ToList();
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

        public PlayerBuilder SetShotsRemaining(int shotsRemaining) {
            this.shotsRemaining = shotsRemaining;
            return this;
        }

        public Player Build()
        {
            return new Player(id, connectionId, cells, ships, gameStatus, isYourTurn, points, shotsRemaining);
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
