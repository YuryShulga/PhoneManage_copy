using PhoneManage.VM;
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
using System.Windows.Shapes;

namespace PhoneManage.ExtraWindows.StreetsWindow
{
    /// <summary>
    /// Interaction logic for StreetsWindow.xaml
    /// </summary>
    public partial class StreetsWindow : Window
    {
        internal MainViewModel mainViewModel;

        public bool CloseFlag;

        internal StreetsWindow(MainViewModel _mainViewModel)
        {
            InitializeComponent();
            mainViewModel = _mainViewModel;
            DataContext = new StreetsWindowViewModal(this);
            CloseFlag = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CloseFlag)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
            this.Hide();
        }
    }
}
