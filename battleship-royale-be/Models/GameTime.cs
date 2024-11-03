using System;

namespace battleship_royale_be.Models
{
    public class GameTime
    {
        private static GameTime _instance;
        private static readonly object _lock = new object();
        private int _timeRemaining;

        private GameTime()
        {
            _timeRemaining = 3600;
        }

        public static GameTime Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameTime();
                    }
                    return _instance;
                }
            }
        }

        public int TimeRemaining
        {
            get => _timeRemaining;
            set => _timeRemaining = value;
        }

        public void UpdateTime(int seconds)
        {
            _timeRemaining = seconds;
        }

        public void DecrementTime()
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining--;
            }
        }
    }
}
