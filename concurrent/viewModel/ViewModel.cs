using Presentation.Model;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Presentation.ViewModel
{
    public class ViewModelMarbles : ViewModelBase
    {
        public ViewModelMarbles(ModelAbstractAPI modelAPI = null)
        {
            StartCommand = new RelayCommand(start);
            StopCommand = new RelayCommand(stop);
            if (modelAPI == null)
            {
                this.modelApi = ModelAbstractAPI.createApi();
            }
            else
            {
                this.modelApi = modelAPI;
            }
        }
        public ViewModelMarbles() : this(null) { }

        private ModelAbstractAPI modelApi;

        private int marbleNumber = 5;
        
        public string MarbleNumber { 
            get => Convert.ToString(marbleNumber);
            set
            {
                Regex regex = new Regex("^([0-9]{1,9})$");
                if (regex.IsMatch(value))
                {
                    marbleNumber = Convert.ToInt32(value);
                    RaisePropertyChanged("MarbleNumber");
                }

            }
        }

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        private ObservableCollection<IEllipse> marbleList;
        public ObservableCollection<IEllipse> MarbleList
        {
            get => marbleList;

            set
            {
                if (value.Equals(marbleList))
                    return;
                marbleList = value;
                RaisePropertyChanged("MarbleList");
            }
        }

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
        public bool StopEnabled { get => !startEnabled; }
        public float CanvasHeight { get; set; }
        public float CanvasWidth { get; set; }

        private void start()
        {
            try
            {
                modelApi.start(marbleNumber, CanvasHeight, CanvasWidth);
            }
            catch (System.ArgumentException)
            {
                return;
            }
            StartEnabled = false;
            MarbleList = modelApi.getEllipses();
        }

        private void stop()
        {
            modelApi.stop();
            StartEnabled = true;
        }

    }
}