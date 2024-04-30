using PhoneManage.ExtraWindows;
using PhoneManage.VM;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneManage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainViewModel mainViewModel;

        public  MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(this);
            DataContext = mainViewModel;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            mainViewModel.findWindow.CloseFlag = true;
            mainViewModel.streetsWindow.CloseFlag = true;
            mainViewModel.streetsWindow.Close();
            mainViewModel.findWindow.Close();
        }
    }
}