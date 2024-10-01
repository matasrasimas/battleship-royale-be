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

        private PlayerBuilder()
        {
        }

        public static PlayerBuilder From(Player player)
        {
            List<Cell> clonedCells = new List<Cell>();
            foreach (Cell cell in player.Cells)
                clonedCells.Add(new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip));

            List<Cell> sortedCells = clonedCells
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();

            List<Ship> clonedShips = new List<Ship>();
            foreach (Ship ship in player.Ships)
            {
                List<Coordinates> clonedCoordinates = new List<Coordinates>();
                foreach (Coordinates coords in ship.Coordinates)
                {
                    clonedCoordinates.Add(new Coordinates(Guid.NewGuid(), coords.Row, coords.Col));
                }
                clonedShips.Add(new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, clonedCoordinates));
            }

            var builder = new PlayerBuilder
            {
                id = player.Id,
                connectionId = player.ConnectionId,
                cells = sortedCells,
                ships = clonedShips,
                gameStatus = player.GameStatus,
                isYourTurn = player.IsYourTurn,
            };
            return builder;
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
            List<Cell> clonedCells = new List<Cell>();
            foreach(Cell cell in cells)
                clonedCells.Add(new Cell(Guid.NewGuid(), cell.Row, cell.Col, cell.IsHit, cell.IsShip));

            List<Cell> sortedCells = clonedCells
                .OrderBy(c => c.Row)
                .ThenBy(c => c.Col)
                .ToList();

            this.cells = sortedCells;
            return this;
        }

        public PlayerBuilder SetShips(List<Ship> ships)
        {
            List<Ship> clonedShips = new List<Ship>();
            foreach (Ship ship in ships)
            {
                List<Coordinates> clonedCoordinates = new List<Coordinates>();
                foreach (Coordinates coords in ship.Coordinates)
                {
                    clonedCoordinates.Add(new Coordinates(Guid.NewGuid(), coords.Row, coords.Col));
                }
                clonedShips.Add(new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, clonedCoordinates));
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

        public Player Build()
        {
            return new Player(id, connectionId, cells, ships, gameStatus, isYourTurn);
        }
    }
}
