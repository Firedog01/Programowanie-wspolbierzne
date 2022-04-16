using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace viewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            MarbleCount = 3;
            StartCommand = new RelayCommand(start);
            StopCommand = new RelayCommand(stop);
        }

        // Data binding
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Console.WriteLine(propertyName+ " changed");
        }

        private int marbleCount;
        public int MarbleCount
        {
            get { return marbleCount; }
            set
            {
                marbleCount = value;
                OnPropertyChanged();
            }
        }

        private float canvasWidth;
        public float CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged();
            }
        }

        private float canvasHeight;
        public float CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged();
            }
        }

        // Command binding
        public ICommand StartCommand { get; set; }
        private void start() 
        {
            Console.WriteLine("Start! Ilość kulek: " + marbleCount);
            Console.WriteLine("Wymiary canvas: " + canvasWidth + " " + canvasHeight);
        }

        public ICommand StopCommand { get; set; }
        private void stop()
        {
            Console.WriteLine("Stop!");
        }
    }
}
