using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManage.Models
{
    /// <summary>
    /// Модель для преобразования sql запроса из бд для основного окна DataGrid
    /// </summary>
    internal class RequestAbonent
    {
        public string Fio { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }

        public string MobilePhone { get; set; }
    }
}
