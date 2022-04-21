using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using viewModel;

namespace view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AllocConsole();
            //DataContext = new ViewModel();
            InitializeComponent();
        }

        // Show console for debugging
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /*
        // Actual canvas size cannot be access using binding
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vm = DataContext as ViewModel;
            vm.CanvasHeight = (float)e.NewSize.Height;
            vm.CanvasWidth = (float)e.NewSize.Width;
        }*/
    }
}
