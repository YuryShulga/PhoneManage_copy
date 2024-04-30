using PhoneManage.Models;
using PhoneManage.VM.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PhoneManage.ExtraWindows
{
    class FindWindowViewModel : INotifyPropertyChanged
    {
        private readonly FindWindow findWindow;

        //команды для кнопки поиск
        public ICommand FindAbonentButtonCommand { get; }

        //привязка к полю ввода номера телефона
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public FindWindowViewModel(FindWindow _findWindow)
        {
            findWindow = _findWindow;
            
            //инициализация команд
            FindAbonentButtonCommand = new AppCommand(FindAbonent);

            
        }

        private void FindAbonent(object parameter)
        {
            if (PhoneNumber == null || PhoneNumber == "") { return; }
            if (findWindow.mainViewModel.FindPhoneNumberInCollection(PhoneNumber))
            {//номер есть 
                findWindow.mainViewModel.UpdateCollectionViewWithSearchWindow(PhoneNumber);
                findWindow.Hide();
            }
            else 
            {
                MessageBox.Show("Нет абонентов, удовлетворяющих критерию поиска", "Поиск абонента",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
               
        }

    }
}
