using System.Diagnostics;

namespace logic
{
    public sealed class Api
    {
        private Api()
        {
            marbleCount = 0;
            marbles = new List<Marble>();
            canvasSize = (1000, 800);
            newMarbleRadius = 50;
            // api loop needs to be created in separate thread
            apiThread = new Thread(ApiLoop);
            apiThread.Start();
        }

        // singleton
        private static Api instance = null;
        private static readonly object padlock = new object();

        public static Api Instance
        {
            get
            {
                lock (padlock) // critical section
                {
                    if (instance == null)
                        instance = new Api();
                    return instance;
                }
            }
        }

        // event
        public event Event.MarbleEventHandler onPosUpdated;

        // api thread
        private static Thread apiThread;

        
        private int marbleCount;
        private List<Marble> marbles;
        private (double, double) canvasSize;
        private double newMarbleRadius;


        public int MarbleCount 
        {
            get { return marbleCount; } 
        }

        public (double, double) CanvasSize 
        { 
            get { return canvasSize; }
            set 
            { 
                if(value.Item1 > newMarbleRadius && value.Item2 > newMarbleRadius)
                {
                    canvasSize = value; 
                }
            }
        }

        
        public double NewMarbleRadius
        {
            get { return newMarbleRadius; }
            set { newMarbleRadius = value; }
        }


        // will create marble at the center of canvas
        public void CreateUnmovingMarble()
        {
            double x = canvasSize.Item1 / 2;
            double y = canvasSize.Item2 / 2;
            marbles.Add(new Marble((0,0), (x, y), newMarbleRadius));
            marbleCount = marbles.Count;
        }

        public void DeleteMarble(int idx)
        {
            marbles.RemoveAt(idx);
            marbleCount = marbles.Count;
        }

        private void ApiLoop()
        {
            int fps = 50;
            int waitTime_ms = 1000 / fps; // 50 fps
            while (true)
            {
                Thread.Sleep(waitTime_ms); 

                // todo update pos

                if(onPosUpdated != null)
                {
                    Event.MarbleInfo[] args = getMarbleArgs();
                    onPosUpdated(this, new Event.MarbleArgs(args));
                }
            }
        }

        private Event.MarbleInfo[] getMarbleArgs()
        {
            Event.MarbleInfo[] ret = new Event.MarbleInfo[marbles.Count];
            int idx = 0;
            foreach (Marble m in marbles)
            {
                (double x, double y) = m.Position;
                ret[idx] = new Event.MarbleInfo(idx, x, y);
                idx++;
            }
            return ret;
        }


    }
}