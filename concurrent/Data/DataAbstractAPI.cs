﻿using System.Threading;
using System.Collections.Generic;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract void createArea(int width, int height, int ballsAmount, int ballRadius);
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

            private readonly object barrier = new object();

            private int queue_cnt = 0;

            private bool active = false;

            private Area area;

            public bool Active { get => active; set => active = value; }
            public override Area Area { get => area; }        

            public override void createArea(int width, int height, int ballsAmount, int ballRadius)
            {
                this.area = new Area(width, height, ballsAmount, ballRadius);
                this.Active = true;
                List<Marble> marbles = getMarbles();

                foreach (Marble marble in marbles) {
                    Thread t = new Thread(() => {
                        while (this.Active)
                        {
                            lock (locked)
                            {
                                marble.move();   
                            }
                            
                            if (Interlocked.CompareExchange(ref queue_cnt, 1, 0) == 0)
                            {
                                Monitor.Enter(barrier);
                                while (queue_cnt != marbles.Count && this.Active){}
                                Interlocked.Decrement(ref queue_cnt);
                                Monitor.Exit(barrier);
                            }
                            else
                            {
                                Interlocked.Increment(ref queue_cnt);
                                Monitor.Enter(barrier);
                                Interlocked.Decrement(ref queue_cnt);
                                Monitor.Exit(barrier);
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

            public override void stop() { 
                this.Active = false;
            }


        }
    }
}