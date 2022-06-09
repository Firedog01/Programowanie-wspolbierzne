using System.Threading;
using System.Collections.Generic;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract void createArea(int width, int height, int marblesCount, int marblesRadius);
        public abstract List<Marble> getMarbles();

        public abstract void stop();

        public abstract Area Area { get; }
        public static DataAbstractAPI createApi()
        {
            return new DataAPI();
        }

        internal sealed class DataAPI : DataAbstractAPI
        {
            private readonly object locked = new object();

            private bool active = false;

            private Area area;

            private Logger logger;

            public bool Active { get => active; set => active = value; }
            public override Area Area { get => area; }

            public override void createArea(int width, int height, int marblesCount, int marbleRadius)
            {
                this.area = new Area(width, height, marblesCount, marbleRadius);
                this.Active = true;
                List<Marble> marbles = getMarbles();

                logger = new Logger(marbles);

                foreach (Marble marble in marbles)
                {
                    Thread t = new Thread(() => {
                        while (this.Active)
                        {
                            lock (locked)
                            {
                                marble.move();
                            }

                            Thread.Sleep(5);
                        }
                    });
                    t.Start();
                }
            }

            public override List<Marble> getMarbles()
            {
                return Area.Marbles;
            }

            public override void stop()
            {
                this.Active = false;
                this.logger.stop();
            }


        }
    }
}
