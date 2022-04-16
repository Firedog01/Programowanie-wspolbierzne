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
        private static long lastTime;

        // random
        private static Random rand;

        private static List<Marble> marbles;
        private static Vector2 canvasSize;
        private static float defaultSpeedValue;
        private static float newMarbleRadius;
        private static bool stop;

        private Api()
        {
            canvasSize = new Vector2(1000, 800);
            newMarbleRadius = 50;
            defaultSpeedValue = 20;
            stop = true;

            lastTime = 0;
            marbles = new List<Marble>();
            rand = new Random();
            
            apiThread = new Thread(ApiLoop); // api loop needs to be created in separate thread
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
        public int MarbleCount { get { return marbles.Count; } }

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
            Vector2 initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
        }
        public void CreateMovingMarble()
        {
            Vector2 initialSpeed = new Vector2(defaultSpeedValue, 0);
            double deg = rand.NextDouble() * 360;
            initialSpeed = Rotate(initialSpeed, deg);
            Vector2 initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
        }

        public void Start()
        {
            foreach (Marble m in marbles)
            {
                if (m.Speed == Vector2.Zero)
                {
                    Vector2 newSpeed = new Vector2(defaultSpeedValue, 0);
                    double deg = rand.NextDouble() * 360;
                    m.Speed = Rotate(newSpeed, deg);
                }
            }
            stop = false;
        }

        public void Stop()
        {
            stop = true;
        }

        public void DeleteMarble(int idx)
        {
            marbles.RemoveAt(idx);
        }


        // ================= MAIN API LOOP =================

        private void ApiLoop()
        {
            GetDeltaTime(); // initialize

            while (true)
            {
                if (stop)
                    continue;

                UpdatePos(GetDeltaTime());

                // send updated position
                if (onPosUpdated != null)
                {
                    Event.MarbleInfo[] args = GetMarbleArgs();
                    onPosUpdated(this, new Event.MarbleArgs(args));
                }
            }
        }

        // ================= PRIVATE METHODS =================

        private void UpdatePos(float delta)
        {
            foreach (Marble m in marbles)
            {
                m.Position = m.Position + m.Speed * delta;
                m.Position = ClampOffCanvas(m);

                // bounce off canvas

                var newSpeed = GetRandomSpeed(m.Speed);
                m.Speed = newSpeed;
            }

            // check colisions
        }
        private static float GetDeltaTime()
        {
            long now = DateTime.Now.Ticks;
            float dT = (now - lastTime); // / 1000
            lastTime = now;
            return dT;
        }

        private Event.MarbleInfo[] GetMarbleArgs()
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

        private static Vector2 GetRandomSpeed(Vector2 speed)
        {
            double deg = rand.NextDouble() * 20 - 10; 
            return Rotate(speed, deg);
        }

        private static Vector2 Rotate(Vector2 v, double degrees)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float)(v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
            );
        }

        private Vector2 ClampOffCanvas(Marble m)
        {
            if(m.Position.X - m.Radius < 0)
                m.Position = new Vector2(m.Radius, m.Position.Y);
            else if(m.Position.X + m.Radius > canvasSize.X)
                m.Position = new Vector2(canvasSize.X - m.Radius, m.Position.Y);

            if (m.Position.Y - m.Radius < 0)
                m.Position = new Vector2(m.Position.X, m.Radius);
            else if (m.Position.Y + m.Radius > canvasSize.Y)
                m.Position = new Vector2(m.Position.X, canvasSize.X - m.Radius);
            return m.Position;
        }
    }
}