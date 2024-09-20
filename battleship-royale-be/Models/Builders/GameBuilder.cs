namespace battleship_royale_be.Models.Builders
{
    public class GameBuilder
    {
        private Guid id;
        private List<Cell> cells;
        private List<Ship> ships;
        private string shotResultMessage;
        private string status;

        private GameBuilder()
        {
        }

        public static GameBuilder From(Game game)
        {
            var builder = new GameBuilder
            {
                id = game.Id,
                cells = new List<Cell>(game.Cells),
                ships = new List<Ship>(game.Ships),
                shotResultMessage = game.ShotResultMessage,
                status = game.Status
            };
            return builder;
        }

        public static GameBuilder DefaultValues()
        {
            return new GameBuilder
            {
                id = Guid.NewGuid(),
                cells = new List<Cell>(),
                ships = new List<Ship>(),
                shotResultMessage = string.Empty,
                status = "IN_PROGRESS"
            };
        }

        public GameBuilder SetId(Guid id)
        {
            this.id = id;
            return this;
        }

        public GameBuilder SetCells(List<Cell> cells)
        {
            this.cells = new List<Cell>(cells);
            return this;
        }

        public GameBuilder SetShips(List<Ship> ships)
        {
            List<Ship> clonedShips = new List<Ship>();
            foreach (Ship ship in ships) {
                List<Coordinates> clonedCoordinates = new List<Coordinates>();
                foreach (Coordinates coords in ship.Coordinates) {
                    clonedCoordinates.Add(new Coordinates(Guid.NewGuid(), coords.Row, coords.Col));
                }
                clonedShips.Add(new Ship(Guid.NewGuid(), ship.HitPoints, ship.IsHorizontal, clonedCoordinates));
            }
            this.ships = clonedShips;
            return this;
        }


        public GameBuilder SetShotResultMessage(string shotResultMessage)
        {
            this.shotResultMessage = shotResultMessage;
            return this;
        }

        public GameBuilder SetStatus(string status)
        {
            this.status = status;
            return this;
        }

        public Game Build()
        {
            return new Game(id, cells, ships, shotResultMessage, status);
        }
    }
}
