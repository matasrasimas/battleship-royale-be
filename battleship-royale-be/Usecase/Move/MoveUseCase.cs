// battleship_royale_be/Usecase/Move/MoveUseCase.cs

using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_royale_be.Usecase.Move
{
    public class MoveUseCase : IMoveUseCase
    {
        private readonly BattleshipAPIContext _context;
        private Game _backup;

        public MoveUseCase(BattleshipAPIContext context)
        {
            _context = context;
        }

        public Game GetBackup()
        {
            return _backup;
        }

        public async Task<Game?> Move(Guid id, string connectionId, int hitPoints)
        {
            try
            {
                // Fetch the game from the database (same logic as before)
                Game? gameToUpdate = await _context.Games
                    .Include(game => game.Players)
                        .ThenInclude(player => player.Cells)
                    .Include(game => game.Players)
                        .ThenInclude(player => player.Ships)
                           .ThenInclude(ship => ship.Coordinates)
                    .Where(g => g.Id == id)
                    .FirstOrDefaultAsync();

                if (gameToUpdate == null)
                {
                    Console.WriteLine($"Game with ID {id} not found.");
                    return null;
                }

                _backup = GameBuilder.From(gameToUpdate).SetPlayers(gameToUpdate.Players).Build();

                // Identify target and attacker players
                Player? targetPlayer = gameToUpdate.Players.FirstOrDefault(player => player.ConnectionId != connectionId);
                Player? attackerPlayer = gameToUpdate.Players.FirstOrDefault(player => player.ConnectionId == connectionId);

                if (targetPlayer == null || attackerPlayer == null)
                {
                    Console.WriteLine("Player not found.");
                    return null;
                }

                // Use the MoveHandler to process the move
                List<Player> playersAfterMove = MoveHandler.HandleMove(attackerPlayer, targetPlayer, hitPoints);

                // Save the new game state to the database (same as before)
                List<Player> updatedPlayersList = playersAfterMove.Select(player => PlayerBuilder.From(player).Build()).ToList();
                Game gameAfterMove = GameBuilder.From(gameToUpdate).SetPlayers(updatedPlayersList).Build();
                _context.Games.Remove(gameToUpdate);
                await _context.Games.AddAsync(gameAfterMove);
                await _context.SaveChangesAsync();

                return gameAfterMove;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Move: {ex.Message}");
                return null;
            }
        }
    }
}
