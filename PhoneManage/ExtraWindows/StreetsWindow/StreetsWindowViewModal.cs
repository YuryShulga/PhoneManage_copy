using PhoneManage.Interfaces;
using PhoneManage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhoneManage.ExtraWindows.StreetsWindow
{
    internal class StreetsWindowViewModal : INotifyPropertyChanged
    {
        private readonly StreetsWindow streetsWindow;

        public ObservableCollection<StreetsAbonentsCount> StreetsAbonentsCountCollection { get; set; }

        public ICollectionView StreetsAbonentsCountCollectionView { get; set; }

        public StreetsWindowViewModal(StreetsWindow _streetsWindow)
        {
            streetsWindow = _streetsWindow;

            //загружаю данные из базы в коллекцию
            StreetsAbonentsCountCollection = new();
            GetStreetsAbonentsCountFromDb();

            //Инициализация CollectionView кллекцией StreetsAbonentsCount
            StreetsAbonentsCountCollectionView = CollectionViewSource.GetDefaultView(StreetsAbonentsCountCollection);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void GetStreetsAbonentsCountFromDb()
        {
            var tempStreetsAbonentsCountCollection = streetsWindow.mainViewModel.repository.SelectStreetsAbonentsCount();

            foreach (var streetsAbonentsCount in tempStreetsAbonentsCountCollection)
            {
                StreetsAbonentsCountCollection.Add(streetsAbonentsCount);
            }
        }


    }
}
