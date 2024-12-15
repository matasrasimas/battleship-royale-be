// battleship_royale_be/Usecase/Move/MoveHandler.cs

using battleship_royale_be.Models;
using System.Collections.Generic;

namespace battleship_royale_be.Usecase.Move
{
    public static class MoveHandler
    {
        public static List<Player> HandleMove(Player attackerPlayer, Player targetPlayer, int hitPoints)
        {
            Console.WriteLine("Delegating move handling to MoveComposite...");
            IMoveComponent moveComposite = new MoveComposite();
            return moveComposite.HandleMove(attackerPlayer, targetPlayer, hitPoints);
        }
    }
}
