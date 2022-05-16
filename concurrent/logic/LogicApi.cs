﻿using System.Collections.ObjectModel;
using System.Numerics;
using logic.Event;

namespace logic
{
    public abstract class LogicApi
    {
        // singleton
        private static LogicApiImplementation implementation;
        public ObservableCollection<Marble> marbles { get; private set; } = new ObservableCollection<Marble>();
        private static readonly object padlock = new object();

        public static LogicApi Implementation
        {
            get
            {
                lock (padlock) // critical section
                {
                    if (implementation == null)
                        implementation = new LogicApiImplementation();
                    return implementation;
                }
            }
        }

        public abstract int MarbleCount { get; }
        public abstract Vector2 CanvasSize { get; set; }
        public abstract float NewMarbleRadius { get; set; }
        public event MarbleEventHandler PosUpdated;

        protected virtual void DoPosUpdated(MarbleInfo[] args)
        {
            // send updated position
            if (PosUpdated != null)
            {
                PosUpdated(this, new MarbleArgs(args));
            }
        }

        

        public abstract void CreateUnmovingMarble();
        public abstract void CreateMovingMarble();
        public abstract void DeleteMarble(int idx);
        public abstract void Start();
        public abstract void Stop();
    }
}
