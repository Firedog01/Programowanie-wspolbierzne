using Data;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class LogicMarble : INotifyPropertyChanged
    {
        private Marble marble;

        public LogicMarble(Marble m) { 
            this.marble = m;
            m.PropertyChanged += update;
        }

        private void update(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "XPos")
            {
                RaisePropertyChanged("XPos");
            }
            else if (e.PropertyName == "YPos")
            {
                RaisePropertyChanged("YPos");
            }
            else if (e.PropertyName == "Radius")
            {
                RaisePropertyChanged("Radius");
            }

        }
        public double XPos {
            get => marble.XPos;
            set
            {
                marble.XPos = value;
                RaisePropertyChanged("XPos");
            }
        }
        public double YPos {
            get => marble.YPos;
            set
            {
                marble.YPos = value;
                RaisePropertyChanged("YPos");
            }
        }
        public double Radius {
            get => marble.Radius;
            set
            {
                if (value > 0)
                {
                    marble.Radius = value;
                    RaisePropertyChanged("Radius");
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
