using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManage.Models
{
    /// <summary>
    /// модель для sql запроса : улица - колличество абонентов на улице
    /// </summary>
    internal class StreetsAbonentsCount
    {
        public string StreetName { get; set; }

        public int AbonentsCount { get; set; }
    }
}
