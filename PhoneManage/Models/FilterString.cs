using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManage.Models
{
	/// <summary>
	/// модель используется для связи элемента TextBlock и ViewModel
	/// </summary>
	internal class FilterString : INotifyPropertyChanged
	{
		private Action FilterChange { get; set;}

		private string _text;

		public string Text
		{
			get { return _text; }
			set
			{
				if (_text != value)
				{
					_text = value;
					OnPropertyChanged(nameof(Text));
					// Вызываем метод при изменении свойства
					FilterChange.Invoke();
				}
			}
		}

		public FilterString (Action filterChange)
		{
			FilterChange = filterChange;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
