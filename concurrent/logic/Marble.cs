namespace logic
{
    public class Marble
    {
        private (double, double) speed;
        public (double, double) Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private (double, double) position;
        public (double, double) Position
        {
            get { return position; }
            set { position = value; }
        }

        private double radius;
        public double Radius 
        { 
            get { return radius; } 
            set { radius = value; } 
        }

        public Marble((double, double) _speed, (double, double) _position, double _radius)
        {
            speed = _speed;
            position = _position;
            radius = _radius;
        }
    }

}