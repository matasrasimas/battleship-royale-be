using System;
using System.Collections.Generic;

namespace battleship_royale_be.DesignPatterns.Composite
{
    public class Fleet : IShipComponent
    {
        private readonly List<IShipComponent> components = new();

        public string FleetName { get; }

        public Fleet(string fleetName)
        {
            FleetName = fleetName;
        }

        public void Add(IShipComponent component)
        {
            components.Add(component);
        }

        public void Move()
        {
            Console.WriteLine($"Fleet {FleetName} is moving:");
            foreach (var component in components)
            {
                component.Move();
            }
        }

        public List<IShipComponent> GetComponents()
        {
            return components;
        }
    }
}
