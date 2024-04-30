using PhoneManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManage.Interfaces
{
    /// <summary>
    /// интерфейс для паттерна репозиторий
    /// </summary>
    internal interface IRepository
    {
        /// <summary>
        /// вызов sql запроса для получения списка всех абонентов бд
        /// </summary>
        /// <returns></returns>
        IEnumerable<RequestAbonent> SelectAllAbonents();

        /// <summary>
        /// Выполняет sql запрос к базе и выводит название улицы и колличество абонентов на ней
        /// </summary>
        /// <returns></returns>
        IEnumerable<StreetsAbonentsCount> SelectStreetsAbonentsCount();

        /// <summary>
        /// заполняет базу данных тестовыми данными
        /// </summary>
        /// <param name="quantity">желаемое колличество строк</param>
        void FillDb(int quantity);

    }
}
