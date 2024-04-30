using Microsoft.Win32;
using PhoneManage.Db;
using PhoneManage.ExtraWindows;
using PhoneManage.ExtraWindows.StreetsWindow;
using PhoneManage.Interfaces;
using PhoneManage.Models;
using PhoneManage.VM.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PhoneManage.VM
{
    internal class MainViewModel : INotifyPropertyChanged
    {
		

		private readonly MainWindow mainWindow;

        public readonly FindWindow findWindow;

		public readonly StreetsWindow streetsWindow;

		//репозиторий - доступ к базе данных
        public readonly IRepository repository;

        /// <summary>
        /// коллекция для отображения информации в DataGrid
        /// </summary>
        public ObservableCollection<RequestAbonent> Abonents { get; set; }

        /// <summary>
        /// коллекция для отображения информации в DataGrid
        /// </summary>
        public ICollectionView AbonentsCollectionView { get; set; }
        
		//Модели для фильтров(на базе TextBox)
		public FilterString FilterString1 { get; set; }
		public FilterString FilterString2 { get; set; }
		public FilterString FilterString3 { get; set; }
		public FilterString FilterString4 { get; set; }
		public FilterString FilterString5 { get; set; }
		public FilterString FilterString6 { get; set; }

		//команды для кнопок
		public ICommand FindWithTelNumberButtonCommand { get; }
		public ICommand SaveDataAsCSVButtonCommand { get; }
		public ICommand ShowStreetsButtonCommand { get; }
		public ICommand ClearAllFiltersButtonCommand { get; }

		public MainViewModel(MainWindow _mainWindow)
        {
			//инициализация репозитория
            repository = new SqliteDb();

            //Заполняю базу тестовыми данными
            repository.FillDb(100);

            mainWindow = _mainWindow;

			//создание вспомогательных окон
            findWindow = new(this);
            streetsWindow = new(this);

            Abonents = new();

			//инициализация команд
			FindWithTelNumberButtonCommand = new AppCommand(FindWithTelNumber);
			SaveDataAsCSVButtonCommand = new AppCommand(SaveDataAsCSV);
			ShowStreetsButtonCommand = new AppCommand(ShowStreets);
			ClearAllFiltersButtonCommand = new AppCommand(ClearAllFilters);

			//инициализация фильтров
			FiltersInit();

            //Загружаю абонентов
            GetAbonentsFromDb();

            //Инициализация CollectionView кллекцией абонентов
            AbonentsCollectionView = CollectionViewSource.GetDefaultView(Abonents);
        }

        //реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

		/// <summary>
		/// инициализатор объектов-фильтров
		/// </summary>
        private void FiltersInit()
        {
			FilterString1 = new(FilterStringChanged);
			FilterString2 = new(FilterStringChanged);
			FilterString3 = new(FilterStringChanged);
			FilterString4 = new(FilterStringChanged);
			FilterString5 = new(FilterStringChanged);
			FilterString6 = new(FilterStringChanged);
		}

		/// <summary>
		/// заргужает список абонентов в ObservableCollection<RequestAbonent> Abonents
		/// </summary>
		private void GetAbonentsFromDb()
        {
            var tempAbonents = repository.SelectAllAbonents();

            foreach (var abonent in tempAbonents)
            {
                Abonents.Add(abonent);
            }
        }

		/// <summary>
		/// обработчик изменения полей-фильтров
		/// </summary>
        private void FilterStringChanged()
        {
			AbonentsCollectionView.Filter = (ra => GetFilter((RequestAbonent)ra));
		}

        private bool GetFilter(RequestAbonent requestAbonent)
		{
			bool result1;
			bool result2;
			bool result3;
			bool result4;
			bool result5;
			bool result6;
			if (FilterString1.Text == null)
			{
				result1 = true;
			}
			else
			{
				result1 = requestAbonent.Fio.ToLower().Contains(FilterString1.Text.ToLower());
			}
			if (FilterString2.Text == null)
			{
				result2 = true;
			}
			else 
			{
				result2 = requestAbonent.Street.ToLower().Contains(FilterString2.Text.ToLower());
			}
			if (FilterString3.Text == null)
			{
				result3 = true;
			}
			else
			{
				result3 = requestAbonent.HouseNumber.ToLower().Contains(FilterString3.Text.ToLower());
			}
			if (FilterString4.Text == null || FilterString4.Text == "")
			{
				result4 = true;
			}
			else
			{
				if (requestAbonent.HomePhone == null)
				{
					result4 = false;
				}
				else 
				{
					result4 = requestAbonent.HomePhone.ToLower().Contains(FilterString4.Text.ToLower());	
				}
			}
			if (FilterString5.Text == null || FilterString5.Text == "")
			{
				result5 = true;
			}
			else
			{
				if (requestAbonent.WorkPhone == null)
				{
					result5 = false;
				}
				else
				{
					result5 = requestAbonent.WorkPhone.ToLower().Contains(FilterString5.Text.ToLower());
				}
			}
			if (FilterString6.Text == null || FilterString6.Text == "")
			{
				result6 = true;
			}
			else
			{
				if (requestAbonent.MobilePhone == null)
				{
					result6 = false;
				}
				else
				{
					result6 = requestAbonent.MobilePhone.ToLower().Contains(FilterString6.Text.ToLower());
				}	
			}
			return result1 && result2 && result3 && result4 && result5 && result6;
		}

        /// <summary>
        /// Обновляет отображение ICollectionView AbonentsCollectionView так, 
		/// что показываются абоненты с переданным номером телефона
        /// </summary>
        /// <param name="phoneNumber">номер телефона абонента</param>
        public void UpdateCollectionViewWithSearchWindow(string phoneNumber)
        {
			FilterString1.Text = "Поиск";
			FilterString2.Text = "по номеру";
			FilterString3.Text = "телефона";
			FilterString4.Text = phoneNumber;
			FilterString5.Text = phoneNumber;
			FilterString6.Text = phoneNumber;
			
            AbonentsCollectionView.Filter = (ra => GetFilterWithSearchWindow((RequestAbonent)ra, phoneNumber));
        }

		/// <summary>
		/// проверяет есть ли в коллекции абонентов человек с указанным номером телефона
		/// </summary>
		/// <param name="phoneNumber">искомый номер телефона</param>
		/// <returns>true - если хотя-бы 1 абонент найден</returns>
		public bool FindPhoneNumberInCollection(string phoneNumber) 
		{
            bool phoneNumberExists = Abonents.Any(abonent =>
				abonent.HomePhone == phoneNumber ||
				abonent.WorkPhone == phoneNumber ||
				abonent.MobilePhone == phoneNumber);
			return phoneNumberExists;
        }

		/// <summary>
		/// предикат который говорит есть ли номер телефона у абонента
		/// </summary>
		/// <param name="requestAbonent"></param>
		/// <param name="phoneNumber"></param>
		/// <returns>true - есть(false - нет)</returns>
		private bool GetFilterWithSearchWindow(RequestAbonent requestAbonent, string phoneNumber)
		{
			if (requestAbonent.MobilePhone == phoneNumber ||
				requestAbonent.HomePhone == phoneNumber ||
				requestAbonent.WorkPhone == phoneNumber) 
			{ 
				return true; 
			}
			return false;
		}

        /// <summary>
        /// обработчик команды нажатия на кнопу Поиск. Выводит на экран данные по номеру телефона
        /// </summary>
        /// <param name="parameter"></param>
        private void FindWithTelNumber(object parameter)
		{
			findWindow.Owner = mainWindow;
			findWindow.ShowDialog();
		}

		/// <summary>
		/// Обработчик нажатия кнопки "Выгрузить CSV"
		/// </summary>
		/// <param name="parameter"></param>
		private void SaveDataAsCSV(object parameter)
		{
			//открытие диалога сохранения
			string filePath = OpenSaveAsDialog();
			//если сохранение отменили - выхожу
			if (filePath == null) { return; }

			// Открываю файл для записи
			using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				// Записываю заголовки столбцов DataGrid
				foreach (var column in mainWindow.MainDataGrid.Columns)
				{
					streamWriter.Write(column.Header);
					streamWriter.Write(",");
				}
				streamWriter.WriteLine();

				// Записываю данные строк DataGrid
				foreach (var item in mainWindow.MainDataGrid.Items)
				{
					foreach (var column in mainWindow.MainDataGrid.Columns)
					{
						var cellValue = (column.GetCellContent(item) as TextBlock)?.Text;
						if (cellValue != null)
						{
							streamWriter.Write(cellValue);
						}
						streamWriter.Write(",");
					}
					streamWriter.WriteLine();
				}
			}
		}

		/// <summary>
		/// Вызывает диалоговое окно SaveAs
		/// </summary>
		/// <returns>возвращает путь к файлу или null(в случае отмены)</returns>
		private string OpenSaveAsDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

            //фильтр для типов файлов
            saveFileDialog.Filter = "Файлы CSV (*.csv)|*.csv|Все файлы (*.*)|*.*";

            //имя файла по умолчанию
            saveFileDialog.FileName = $"{DateTime.Now}.csv";

            // устанавливаю начальную директорию - раб. стол
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            saveFileDialog.Title = "Сохранить файл как..."; 

			string filePath = null;

			if (saveFileDialog.ShowDialog() == true)
			{
				filePath = saveFileDialog.FileName;
			}
			return filePath;
		}

		/// <summary>
		/// обработчик команды кнопки Показать улицы
		/// </summary>
		/// <param name="parameter"></param>
		private void ShowStreets(object parameter)
		{
			streetsWindow.Owner = mainWindow;
			streetsWindow.ShowDialog();
        }

		/// <summary>
		/// Очищает все TextBox фильтров (и модели тоже)
		/// </summary>
		/// <param name="parameter"></param>
		private void ClearAllFilters(object parameter)
		{
			FilterString1.Text = null;
			FilterString2.Text = null;
			FilterString3.Text = null;
			FilterString4.Text = null;
			FilterString5.Text = null;
			FilterString6.Text = null;

			//запускаю сброс фильтров, чтобы показать все записи
            AbonentsCollectionView.Filter = (ra => GetFilter((RequestAbonent)ra));
        }
	}
}
