using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI createApi(DataAbstractAPI dataAbstractAPI = null)
        {
            return new LogicAPI(dataAbstractAPI);
        }

        public abstract List<LogicMarble> getMarbles();
        public abstract void start(int width, int height, int marlesAmount, int marbleRadius);
        public abstract void stop();

        internal sealed class LogicAPI : LogicAbstractAPI
        {
            private DataAbstractAPI dataAPI;

            bool active = false;

            private List<LogicMarble> marbles = new List<LogicMarble>();
            
            public bool Active { get => active; set => active = value; }
            public List<LogicMarble> Balls { get => marbles; set => marbles = value; }

            internal LogicAPI(DataAbstractAPI dataAbstractAPI = null)
            {
                if (dataAbstractAPI == null)
                {
                    this.dataAPI = DataAbstractAPI.createApi();
                }
                else
                {
                    this.dataAPI = dataAbstractAPI;
                }
            }

            public override void stop()
            {
                dataAPI.stop();
                this.Balls.Clear();
            }

            public override void start(int width, int height, int ballsAmount, int ballRadius) {
                dataAPI.createArea(width, height, ballsAmount, ballRadius);
                foreach (Marble b in dataAPI.getMarbles()) {
                    this.Balls.Add(new LogicMarble(b));
                    b.PropertyChanged += update;
                }
            }
            public override List<LogicMarble> getMarbles()
            {
                return this.Balls;
            }

            private void update(object sender, PropertyChangedEventArgs e)
            {
                Marble marble = (Marble)sender;
                if (e.PropertyName == "Position")
                {
                    collide(marble);
                }

            }

            private void borderCollision(Marble main) {
                if ((main.XPos + main.Radius) >= dataAPI.Area.Width)
                {
                    main.xSpeed = -main.xSpeed;
                    main.XPos = dataAPI.Area.Width - main.Radius;
                }
                if ((main.XPos - main.Radius) <= 0)
                {
                    main.xSpeed = -main.xSpeed;
                    main.XPos = main.Radius;
                }
                if ((main.YPos + main.Radius) >= dataAPI.Area.Height)
                {
                    main.ySpeed = -main.ySpeed;
                    main.YPos = dataAPI.Area.Height - main.Radius;
                }
                if ((main.YPos - main.Radius) <= 0)
                {
                    main.ySpeed = -main.ySpeed;
                    main.YPos = main.Radius;
                }
            }

            private void marbleCollision(Marble main) {
                foreach (Marble m in dataAPI.getMarbles())
                {
                    if (m == main)
                    {
                        continue;
                    }
                    double xCol = m.XPos - main.XPos;
                    double yCol = m.YPos - main.YPos;
                    double distance = Math.Sqrt((xCol * xCol) + (yCol * yCol));
                    if (distance <= (main.Radius + m.Radius))
                    {
                        double newB = ((m.xSpeed * (m.Weight - main.Weight) + (2 * main.Weight * main.xSpeed)) / (m.Weight + main.Weight));
                        main.xSpeed = ((main.xSpeed * (main.Weight - m.Weight) + (2 * m.Weight * m.xSpeed)) / (m.Weight + main.Weight));
                        m.xSpeed = newB;

                        newB = ((m.ySpeed * (m.Weight - main.Weight)) + (2 * main.Weight * main.ySpeed) / (m.Weight + main.Weight));
                        main.ySpeed = ((main.ySpeed * (main.Weight - m.Weight)) + (2 * m.Weight * m.ySpeed) / (m.Weight + main.Weight));
                        m.ySpeed = newB;
                    }
                }
            }
            private void collide(Marble main) {
                borderCollision(main);
                marbleCollision(main);                
            }
        }
    }
}
