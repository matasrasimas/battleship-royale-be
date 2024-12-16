// battleship_royale_be/Usecase/Move/Components/ShipMovementComponent.cs

using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Models.Converters;
using battleship_royale_be.Usecase.Shoot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace battleship_royale_be.Usecase.Move.Components
{
    public class ShipMovementComponent : IMoveComponent
    {
        public List<Player> HandleMove(Player attackerPlayer, Player targetPlayer, int hitPoints)
        {
            Console.WriteLine("Handling ship movement...");
            var shipsToMove = attackerPlayer.Ships.Where(ship => ship.HitPoints == hitPoints).ToList();
            if (shipsToMove.Count == 0)
            {
                Console.WriteLine($"No ships found in the attacker player's fleet with hitPoints: {hitPoints}");
                return new List<Player>();
            }

            Console.WriteLine("Ships selected for movement:");
            foreach (var ship in shipsToMove)
            {
                Console.WriteLine($"Ship with hitPoints: {ship.HitPoints}");
            }

            Board board = new Board(
                (Cell[,])GridConverter.FromListToArray(attackerPlayer.Cells).Clone(),
                new List<Ship>(attackerPlayer.Ships)
            );

            Player updatedAttackerPlayer = ShipsMover.MoveShips(attackerPlayer, shipsToMove, board.Grid);
            Console.WriteLine("Updated attacker player's fleet after the move:");
            foreach (var ship in updatedAttackerPlayer.Ships)
            {
                Console.WriteLine($"Ship with hitPoints: {ship.HitPoints}");
            }

            bool isDefeated = !updatedAttackerPlayer.Ships.Any();
            Console.WriteLine($"Updated attacker player's game status: {(isDefeated ? "LOST" : "IN_PROGRESS")}");

            return new List<Player> {
                PlayerBuilder
                    .From(targetPlayer)
                    .SetIsYourTurn(true)
                    .Build(),
                PlayerBuilder
                    .From(updatedAttackerPlayer)
                    .SetCells(GridConverter.FromArrayToList(board.Grid))
                    .SetShips(updatedAttackerPlayer.Ships)
                    .SetIsYourTurn(false)
                    .Build()
            };
        }
    }
}
