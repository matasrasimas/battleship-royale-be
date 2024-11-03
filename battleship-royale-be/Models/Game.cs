using System;
using System.Collections.Generic;

namespace battleship_royale_be.Models
{
    public class Game
    {
        private static Game _instance;

        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public string ShotResultMessage { get; set; }

        private Game(Guid Id, List<Player> Players, string ShotResultMessage)
        {
            this.Id = Id;
            this.Players = Players;
            this.ShotResultMessage = ShotResultMessage;
        }

        public Game() { }

        public Game(Guid Id, List<Player> Players, string ShotResultMessage)
        {
            this.Id = Id;
            this.Players = Players;
            this.ShotResultMessage = ShotResultMessage;
        }

        public static Game Instance(Guid Id, List<Player> Players, string ShotResultMessage)
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game(Id, Players, ShotResultMessage);
                }
                return _instance; 
            }
        }
    }
}
