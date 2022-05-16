using logic;
using logic.Event;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace model
{
    public class Model
    {
        //singleton
        private static Model instance = null;
        private static readonly object padlock = new object();

        //
        public static ObservableCollection<Marble> Marbles { get; set; }
        public static int marbleCount { get; set; }

        private Model() 
        {
            Marbles = new ObservableCollection<Marble>();
            LogicApi.Implementation.PosUpdated += new MarbleEventHandler(OnPosUpdated);
        }

        public static Model Instance
        {
            get
            {
                lock (padlock) // critical section
                {
                    if (instance == null)
                        instance = new Model();
                    return instance;
                }
            }
        }

        public void init()
        {
            // nothing
        }

        public void OnPosUpdated(object source, MarbleArgs args)
        {
            
        }

        public void startMarbles(int marbles, float canvasWidth, float canvasHeight)
        {
            LogicApi.Implementation.CanvasSize = new Vector2(canvasWidth, canvasHeight);
            LogicApi.Implementation.marbles.Clear();
            marbleCount = marbles;
            for(int i=0; i<marbleCount; i++)
            {
                LogicApi.Implementation.CreateMovingMarble();
            }
            LogicApi.Implementation.Start();
            Marbles = new ObservableCollection<Marble>(LogicApi.Implementation.marbles);
        }

        public void stopMarbles()
        {
            LogicApi.Implementation.Stop();
        }

    }
}