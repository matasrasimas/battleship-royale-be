using battleship_royale_be.DesignPatterns.Memento;
using battleship_royale_be.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace battleship_royale_be.Data
{
    public class BattleshipAPIContext : DbContext
    {
        public BattleshipAPIContext(DbContextOptions<BattleshipAPIContext> options) : base(options) {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<Caretaker> Caretakers { get; set; }
    }
}
