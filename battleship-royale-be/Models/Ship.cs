using battleship_royale_be.DesignPatterns.Composite;
using battleship_royale_be.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace battleship_royale_be.Models
{
    public class Ship : IShipComponent, ICloneable
    {
        public Guid Id { get; set; }
        public int HitPoints { get; set; }
        public bool IsHorizontal { get; set; }
        public bool CanMove { get; set; }
        public List<Coordinates> Coordinates { get; set; }
        public string ImagePath { get; set; }

        public Ship() { }

        public Ship(Guid Id, int HitPoints, bool IsHorizontal, bool CanMove, List<Coordinates> Coordinates, string ImagePath = "")
        {
            this.Id = Id;
            this.HitPoints = HitPoints;
            this.IsHorizontal = IsHorizontal;
            this.CanMove = CanMove;
            this.Coordinates = Coordinates;
            this.ImagePath = ImagePath;
        }

        public Coordinates calculateStartCoordinates()
        {
            return IsHorizontal
                ? Coordinates.OrderBy(coords => coords.Col).First()
                : Coordinates.OrderBy(coords => coords.Row).First();
        }

        public Coordinates CalculateEndCoordinates(Coordinates startCoords)
        {
            int endRow = CalculateShipEndRow(startCoords);
            int endCol = CalculateShipEndColumn(startCoords);
            return new Coordinates(Guid.NewGuid(), endRow, endCol);
        }

        private int CalculateShipEndColumn(Coordinates startCoords)
        {
            return IsHorizontal ? startCoords.Col + HitPoints - 1 : startCoords.Col;
        }

        private int CalculateShipEndRow(Coordinates startCoords)
        {
            return IsHorizontal ? startCoords.Row : startCoords.Row + HitPoints - 1;
        }

        public Board Damage(Board board)
        {
            var updatedShip = ShipBuilder
                .From(this)
                .SetHitPoints(this.HitPoints - 1)
                .Build();
            var updatedShipsList = new List<Ship>(board.Ships);
            var shipIndex = updatedShipsList.IndexOf(this);
            if (shipIndex != -1)
            {
                updatedShipsList[shipIndex] = updatedShip;
            }

            return BoardBuilder
                .From(board)
                .SetShips(updatedShipsList)
                .Build();
        }
        public Board Destroy(Board board)
        {
            var updatedShipsList = new List<Ship>(board.Ships);
            updatedShipsList.Remove(this);

            return BoardBuilder
                .From(board)
                .SetShips(updatedShipsList)
                .Build();
        }

        public void MoveByHitPoints(int hitPoints)
        {
            if (!CanMove)
            {
                Console.WriteLine($"Ship {Id} cannot move or isn't part of the fleet.");
                return;
            }
            if (HitPoints == hitPoints)
            {
                var random = new Random();
                int deltaRow = random.Next(-1, 2);
                int deltaCol = random.Next(-1, 2);
                for (int i = 0; i < Coordinates.Count; i++)
                {
                    Coordinates[i] = new Coordinates(
                        Coordinates[i].Id,
                        Coordinates[i].Row + deltaRow,
                        Coordinates[i].Col + deltaCol
                    );
                }
                Console.WriteLine($"Ship {Id} moved to new coordinates.");
            }
        }

        public object ShallowClone()
        {
            return (Ship)this.MemberwiseClone();
        }

        public object Clone()
        {
            Ship clone = (Ship)this.MemberwiseClone();
            clone.Coordinates = this.Coordinates.Select(item => (Coordinates)item.Clone()).ToList();
            return clone;
        }
    }
}
