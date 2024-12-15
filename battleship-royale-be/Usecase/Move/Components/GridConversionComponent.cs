// battleship_royale_be/Usecase/Move/Components/GridConversionComponent.cs

using battleship_royale_be.Models;
using battleship_royale_be.Models.Converters;
using System;
using System.Collections.Generic;

namespace battleship_royale_be.Usecase.Move.Components
{
    public class GridConversionComponent : IMoveComponent
    {
        public List<Player> HandleMove(Player attackerPlayer, Player targetPlayer, int hitPoints)
        {
            Console.WriteLine("Converting attacker player's grid to array...");
            Cell[,] grid = GridConverter.FromListToArray(attackerPlayer.Cells);
            if (grid == null)
            {
                Console.WriteLine("Error: Grid conversion failed. The grid is null.");
                return new List<Player>(); // return empty list to continue process in composite pattern
            }
            else
            {
                Console.WriteLine("Grid conversion successful. Grid dimensions: " +
                                  $"{grid.GetLength(0)}x{grid.GetLength(1)}");
                return null; // Continue the composite flow
            }
        }
    }
}
