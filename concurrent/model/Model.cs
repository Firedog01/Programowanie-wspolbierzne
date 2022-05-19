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
        public static ObservableCollection<IEllipse> ellipses = new ObservableCollection<IEllipse>();
        internal ObservableCollection<IEllipse> Ellipses { get => ellipses; set => ellipses = value; }
        public static int marbleCount { get; set; }

        private Model() 
        {
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
            logic.LogicApi.Implementation.Start();
        }

        public void stopMarbles()
        {
            LogicApi.Implementation.Stop();
        }
        
        public ObservableCollection<IEllipse> getEllipses()
        {
            List<logic.Marble> balls = logic.LogicApi.Implementation.getBalls();
            Ellipses.Clear();
            foreach (logic.Marble b in balls)
            {
                Ellipses.Add(new Ellipse(b));
            }
            return Ellipses;
        }

    }
}