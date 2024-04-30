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

namespace PhoneManage.ExtraWindows
{
	/// <summary>
	/// Interaction logic for FindWindow.xaml
	/// </summary>
	public partial class FindWindow : Window
	{
		public bool CloseFlag;

		internal  MainViewModel mainViewModel;
		
		internal FindWindow(MainViewModel _mainViewModel)
		{
			InitializeComponent();
			DataContext = new FindWindowViewModel(this);
			mainViewModel = _mainViewModel;
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
