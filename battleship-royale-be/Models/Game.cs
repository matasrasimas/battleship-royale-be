﻿namespace battleship_royale_be.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public string ShotResultMessage { get; set; }

        public Game() { }

        public Game(Guid Id, List<Player> Players, string ShotResultMessage)
        {
            this.Id = Id;
            this.Players = Players;
            this.ShotResultMessage = ShotResultMessage;
        }
    }
}