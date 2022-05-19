using logic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace model
{
    internal class Ellipse : IEllipse
    {
        private double width;
        private double height;
        private double x;
        private double y;
        internal Ellipse(logic.Marble b)
        {
            this.width = 2 * b.Radius;
            this.height = 2 * b.Radius;
            this.x = b.Position.X - b.Radius;
            this.y = b.Position.Y - b.Radius;
            b.PropertyChanged += update;
        }

        private void update(object sender, PropertyChangedEventArgs e)
        {
            logic.Marble ball = (logic.Marble)sender;
            if (e.PropertyName == "XPos")
            {
                this.X = ball.Position.X - ball.Radius;
            }
            else if (e.PropertyName == "YPos")
            {
                this.Y = ball.Position.Y - ball.Radius;
            }
            else if (e.PropertyName == "Radius")
            {
                this.Width = 2 * ball.Radius;
                this.Height = 2 * ball.Radius;
            }
            else
            {
                throw new ArgumentException();
            }

        }

        public double Width
        {
            get => width;
            set
            {
                width = value;
                RaisePropertyChanged("Width");
            }
        }
        public double Height
        {
            get => height;
            set
            {
                height = value;
                RaisePropertyChanged("Height");
            }
        }
        public double X
        {
            get => x;
            set
            {
                x = value;
                RaisePropertyChanged("X");
            }
        }
        public double Y
        {
            get => y;
            set
            {
                y = value;
                RaisePropertyChanged("Y");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}