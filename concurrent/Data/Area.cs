using System;
using System.Collections.Generic;

namespace Data
{
    public class Area
    {
        private readonly float width;
        private readonly float height;
        private readonly List<Marble> balls = new List<Marble>();

        public float Width => width;
        public float Height => height;
        public List<Marble> Marbles => balls;
        
        public Area(float width, float height, int marbleAmount, int marbleRadius)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.width = width;
            this.height = height;
            createMarbles(marbleAmount, marbleRadius);
        }

        private Marble generateMarble(float radius) {
            Random rand = new Random();
            bool ok = true;
            float x = radius;
            float y = radius;
            do
            {
                ok = true;
                x = rand.Next((int)(radius), (int)(this.width - radius));
                y = rand.Next((int)(radius), (int)(this.height - radius));
                foreach (Marble m in this.Marbles)
                {
                    double distance = Math.Sqrt(((m.XPos - x) * (m.XPos - x)) + ((m.YPos - y) * (m.YPos - y)));
                    if (distance <= m.Radius + radius)
                    {
                        ok = false;
                        break;
                    };
                }
                if (!ok)
                {
                    continue;
                };
                ok = true;

            } while (!ok);
            double w = 1;
            return new Marble(x, y, w * radius, w);
        }
        public void createMarbles(int amount, int radius)
        {
            if (2 * radius > width || 2 * radius > height || radius <= 0 || amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            for (int i = 0; i < amount; i++)
            {
                Marble marble = generateMarble(radius);
                this.Marbles.Add(marble);
            }
        }
    }
}
