using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace logic
{
    public class Marble : INotifyPropertyChanged
    {
        private Vector2 speed;
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private float radius;
        public float Radius 
        { 
            get { return radius; } 
            set { radius = value; } 
        }

        public Marble(Vector2 _speed, Vector2 _position, float _radius)
        {
            speed = _speed;
            position = _position;
            radius = _radius;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}