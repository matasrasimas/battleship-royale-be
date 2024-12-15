// battleship_royale_be/Usecase/Move/IMoveComponent.cs

using battleship_royale_be.Models;
using System.Collections.Generic;

namespace battleship_royale_be.Usecase.Move
{
    public interface IMoveComponent
    {
        List<Player> HandleMove(Player attackerPlayer, Player targetPlayer, int hitPoints);
    }
}
