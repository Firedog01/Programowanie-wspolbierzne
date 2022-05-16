using System.Diagnostics;
using System.Numerics;
using data;

namespace logic
{
    internal sealed class LogicApiImplementation : LogicApi
    {
        // event
        public event Event.MarbleEventHandler PosUpdated;

        // api thread
        private static Thread apiThread;

        // time
        private static long lastTime;

        // random
        private static Random rand;

        DataApi dataApi;

        public static List<Marble> marbles { get; private set; }
        private static Vector2 canvasSize;
        private static float defaultSpeedValue;
        private static float newMarbleRadius;
        private static bool stop;

        public LogicApiImplementation()
        {
            canvasSize = new Vector2(1000, 800);
            newMarbleRadius = 50;
            defaultSpeedValue = 20;
            stop = true;

            dataApi = null;

            lastTime = 0;
            marbles = new List<Marble>();
            rand = new Random();
            
            apiThread = new Thread(ApiLoop); // api loop needs to be created in separate thread
            apiThread.Start();
        }


        // ================= ACCESS METHODS =================

        public override int MarbleCount { get { return marbles.Count; } }

        public override Vector2 CanvasSize 
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
        
        public override float NewMarbleRadius
        {
            get { return newMarbleRadius; }
            set { newMarbleRadius = value; }
        }

        // ================= AVAILABLE API METHODS =================

        // will create marble at the center of canvas
        public override void CreateUnmovingMarble()
        {
            Vector2 initialSpeed = new Vector2(0, 0);
            Vector2 initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
        }

        public override void CreateMovingMarble()
        {
            Vector2 initialSpeed = new Vector2(defaultSpeedValue, 0);
            double deg = rand.NextDouble() * 360;
            initialSpeed = Rotate(initialSpeed, deg);
            Vector2 initialPos = canvasSize / 2;
            marbles.Add(new Marble(initialSpeed, initialPos, newMarbleRadius));
        }

        public override void Start()
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

        public override void Stop()
        {
            stop = true;
        }

        public override void DeleteMarble(int idx)
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

                Event.MarbleInfo[] args = GetMarbleArgs();
                DoPosUpdated(args);
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
            float dT = (now - lastTime) / 1000.0f;
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