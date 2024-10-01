namespace battleship_royale_be.Models.Builders
{
    public class GameBuilder
    {
        private Guid id;
        private List<Player> players;
        private string shotResultMessage;

        private GameBuilder()
        {
        }

        public static GameBuilder From(Game game)
        {
            List<Player> clonedPlayers = new List<Player>();

            foreach (Player player in game.Players) {
                clonedPlayers.Add(PlayerBuilder.From(player).Build());
            }

            var builder = new GameBuilder
            {
                id = game.Id,
                players = clonedPlayers,
                shotResultMessage = game.ShotResultMessage,
            };
            return builder;
        }

        public static GameBuilder DefaultValues()
        {
            return new GameBuilder
            {
                id = Guid.NewGuid(),
                players = new List<Player>(),
                shotResultMessage = string.Empty,
            };
        }

        public GameBuilder SetId(Guid id)
        {
            this.id = id;
            return this;
        }

        public GameBuilder SetPlayers(List<Player> players) {
            List<Player> clonedPlayers = new List<Player>();
            foreach (Player player in players)
                clonedPlayers.Add(PlayerBuilder.From(player).Build());

            this.players = clonedPlayers;
            return this;
        }

        public GameBuilder SetShotResultMessage(string shotResultMessage)
        {
            this.shotResultMessage = shotResultMessage;
            return this;
        }

        public Game Build()
        {
            return new Game(id, players, shotResultMessage);
        }
    }
}
