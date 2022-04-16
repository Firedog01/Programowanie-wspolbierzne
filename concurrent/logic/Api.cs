using System.Diagnostics;
using System.Numerics;

namespace logic
{
    public sealed class Api
    {
        // singleton
        private static Api instance;
        private static readonly object padlock = new object();

        // event
        public event Event.MarbleEventHandler onPosUpdated;

        // api thread
        private static Thread apiThread;

        // time
        private static long lastTime = 0;

        private static Random rand;

        private int marbleCount;
        private List<Marble> marbles;
        private Vector2 canvasSize;
        private float dafaultSpeedValue;
        private float newMarbleRadius;

        private Api()
        {
            marbleCount = 0;
            marbles = new List<Marble>();
            canvasSize = new Vector2(1000, 800);
            newMarbleRadius = 50;
            rand = new Random();
            // api loop needs to be created in separate thread
            apiThread = new Thread(ApiLoop);
            apiThread.Start();
        }

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


        // ================= ACCESS METHODS =================
        public int MarbleCount { get { return marbleCount; } }

        public Vector2 CanvasSize 
        { 
            get { return canvasSize; }
            set 
            { 
                if(value.X > newMarbleRadius && value.Y > newMarbleRadius)
                {
                    canvasSize = value; 
                }
            }
        }
        
        public float NewMarbleRadius
        {
            get { return newMarbleRadius; }
            set { newMarbleRadius = value; }
        }

        // ================= AVAILABLE API METHODS =================

        // will create marble at the center of canvas
        public void CreateUnmovingMarble()
        {
            Vector2 initialSpeed = new Vector2(0, 0);
            Vector2 initialPos;
            initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
            marbleCount = marbles.Count;
        }
        public void CreateMovingMarble()
        {
            Vector2 initialSpeed = new Vector2(0, 0);
            Vector2 initialPos;
            initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
            marbleCount = marbles.Count;
        }

        public void Start()
        {

        }

        public void DeleteMarble(int idx)
        {
            marbles.RemoveAt(idx);
            marbleCount = marbles.Count;
        }


        // ================= MAIN API LOOP =================

        private void ApiLoop()
        {
            float delta;
            GetDeltaTime(); // initialize

            while (true)
            {
                delta = GetDeltaTime();
                updatePos(delta);

                // send updated position
                if (onPosUpdated != null)
                {
                    Event.MarbleInfo[] args = getMarbleArgs();
                    onPosUpdated(this, new Event.MarbleArgs(args));
                }
            }
        }

        // ================= PRIVATE METHODS =================
        private static float GetDeltaTime()
        {
            long now = DateTime.Now.Ticks;
            float dT = (now - lastTime); // / 1000
            lastTime = now;
            return dT;
        }

        private Event.MarbleInfo[] getMarbleArgs()
        {
            Event.MarbleInfo[] ret = new Event.MarbleInfo[marbles.Count];
            int idx = 0;
            foreach (Marble m in marbles)
            {
                ret[idx] = new Event.MarbleInfo(idx, m.Position.X, m.Position.Y);
                idx++;
            }
            return ret;
        }

        private void updatePos(float delta)
        {
            foreach (Marble m in marbles)
            {
                var newPos = m.Position + m.Speed * delta;
                m.Position = newPos;
                ClampOffCanvas(m);
                var newSpeed = getRandomSpeed(m.Speed);

            }
        }

        private static Vector2 getRandomSpeed(Vector2 speed)
        {

            double deg = rand.NextDouble(); 
            return Rotate(speed, deg);
        }

        private static Vector2 Rotate(this Vector2 v, double degrees)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float)(v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
            );
        }

        private void ClampOffCanvas(Marble m)
        {
            if(m.Position.X - m.Radius < 0)
                m.Position = new Vector2(m.Radius, m.Position.Y);
            else if(m.Position.X + m.Radius > canvasSize.X)
                m.Position = new Vector2(canvasSize.X - m.Radius, m.Position.Y);

            if (m.Position.Y - m.Radius < 0)
                m.Position = new Vector2(m.Position.X, m.Radius);
            else if (m.Position.Y + m.Radius > canvasSize.Y)
                m.Position = new Vector2(m.Position.X, canvasSize.X - m.Radius);

        }
    }
}