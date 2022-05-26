using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModel.ViewModelMarbles();
            InitializeComponent();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vm = DataContext as ViewModel.ViewModelMarbles;
            vm.CanvasHeight = (float)e.NewSize.Height;
            vm.CanvasWidth = (float)e.NewSize.Width;
        }
    }
}
