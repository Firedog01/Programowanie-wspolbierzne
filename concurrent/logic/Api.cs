namespace logic
{
    public sealed class Api
    {
        Api()
        {
            marbleCount = 0;
            marbles = new List<Marble>();
            canvasSize = (1000, 800);
            newMarbleRadius = 50;
        }

        // singleton
        private static Api instance = null;
        private static readonly object padlock = new object();

        public static Api Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new Api();
                    return instance;
                }
            }
        }

        // event
        public event Event.Handler onPosUpdated;

        // 
        private int marbleCount;
        public int MarbleCount { get { return marbleCount; } }

        private List<Marble> marbles;

        private (double, double) canvasSize;
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

        private double newMarbleRadius;
        public double NewMarbleRadius
        {
            get { return newMarbleRadius; }
            set { newMarbleRadius = value; }
        }


        // will create marble at the center of canvas
        public void CreateUnmovingMarble()
        {
            marbleCount++;
            double x = canvasSize.Item1 / 2;
            double y = canvasSize.Item2 / 2;
            marbles.Add(new Marble((0,0), (x, y), newMarbleRadius));
        }

        
    }
}