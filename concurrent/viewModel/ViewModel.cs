using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel;
using model;
using System.Numerics;
using logic;

namespace viewModel
{
    public class ViewModel : INotifyPropertyChanged
    {

        public ViewModel()
        {
            model.Model.Instance.init();

            MarbleCount = 3;
            StartCommand = new RelayCommand(start);
            StopCommand = new RelayCommand(stop);
        }

        // Data binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int marbleCount;
        public int MarbleCount
        {
            get { return marbleCount; }
            set
            {
                marbleCount = value;
                RaisePropertyChanged();
                Console.WriteLine("Zmieniono ilosc: " + value);
            }
        }

        /*
        private bool startEnabled = true;
        public bool StartEnabled
        {
            get => startEnabled;
            set
            {
                startEnabled = value;
                RaisePropertyChanged("StartEnabled");
                RaisePropertyChanged("StopEnabled");
            }
        }
        public bool StopEnabled { get => !startEnabled; }*/

        private float canvasWidth;
        public float CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                RaisePropertyChanged();
            }
        }

        private float canvasHeight;
        public float CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Marble> marbleList;
        public ObservableCollection<Marble> MarbleList
        {
            get => marbleList;

            set
            {
                marbleList = value;
                RaisePropertyChanged("MarbleList");
            }
        }

        // Command binding
        public ICommand StartCommand { get; set; }
        private void start() 
        {
            Model.Instance.startMarbles(marbleCount, canvasWidth, canvasHeight);
            Console.WriteLine("Start! Ilość kulek: " + marbleCount);
            MarbleList = Model.Marbles;
            Console.WriteLine("Start! Ilość kulek: " + marbleCount);
            Console.WriteLine("Wymiary canvas: " + canvasWidth + " " + canvasHeight);
        }

        public ICommand StopCommand { get; set; }
        private void stop()
        {
            Model.Instance.stopMarbles();
            Console.WriteLine("Stop!");
        }
    }
}
