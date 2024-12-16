using System.Collections.Generic;

namespace battleship_royale_be.DesignPatterns.Composite
{
    public class Fleet : IShipComponent
    {
        private List<IShipComponent> _components = new List<IShipComponent>();

        public void Add(IShipComponent component)
        {
            _components.Add(component);
        }

        public void Remove(IShipComponent component)
        {
            _components.Remove(component);
        }

        public void MoveByHitPoints(int hitPoints)
        {
            foreach (var component in _components)
            {
                component.MoveByHitPoints(hitPoints);
            }
        }
    }
}
