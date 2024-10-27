namespace battleship_royale_be.Models.Observer
{
    public abstract class Subject
    {
        private List<IObserver> observers = [];

        public void Attach(IObserver unit)
        {
            observers.Add(unit);
        }

        public void Deattach(IObserver unit)
        {
            observers.Remove(unit);
        }

        public void ReceiveFromClient(string msg)
        {
            NotifyAll(msg);
        }

        public void NotifyAll(string msg)
        {
            foreach (IObserver unit in observers)
            {
                unit.Update(msg);
            }
        }
    }
}