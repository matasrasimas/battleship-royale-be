// battleship_royale_be/Usecase/Move/MoveComposite.cs

using System;
using System.Collections.Generic;
using battleship_royale_be.Models;

namespace battleship_royale_be.Usecase.Move
{
    public class MoveComposite : IMoveComponent
    {
        private readonly List<IMoveComponent> _components;

        public MoveComposite()
        {
            _components = new List<IMoveComponent>
            {
                new Components.GridConversionComponent(),
                new Components.ShipMovementComponent(),
            };
        }

        public List<Player> HandleMove(Player attackerPlayer, Player targetPlayer, int hitPoints)
        {
            Console.WriteLine("Executing MoveComposite...");
            List<Player> result = null;

            foreach (var component in _components)
            {
                result = component.HandleMove(attackerPlayer, targetPlayer, hitPoints);
                if (result != null && result.Count > 0)
                {
                    break; // If any component returns a result, we stop the execution
                }
            }

            Console.WriteLine("MoveComposite execution finished.");
            return result;
        }
    }
}
