using battleship_royale_be.DesignPatterns.Composite;
using System.Collections.Generic;

namespace battleship_royale_be.Models
{
    public class ShipGroup : IShipComponent
    {
        private List<IShipComponent> _ships = new List<IShipComponent>();
        public List<IShipComponent> Ships { get; private set; }

        public ShipGroup(List<IShipComponent> ships)
        {
            Ships = ships ?? throw new ArgumentNullException(nameof(ships));
        }
        public void Add(IShipComponent ship)
        {
            _ships.Add(ship);
        }

        public void Remove(IShipComponent ship)
        {
            _ships.Remove(ship);
        }

        public Board Damage(Board board)
        {
            foreach (var ship in _ships)
            {
                board = ship.Damage(board);
            }
            return board;
        }

        // Implement Destroy method for ShipGroup
        public Board Destroy(Board board)
        {
            foreach (var ship in _ships)
            {
                board = ship.Destroy(board);
            }
            return board;
        }
    }
}
