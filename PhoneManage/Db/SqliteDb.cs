using Dapper;
using Microsoft.Data.Sqlite;
using PhoneManage.Interfaces;
using PhoneManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManage.Db
{
    internal class SqliteDb : IRepository
    {
        private readonly string ConnectionString = (@"Data Source=Db\Subscribers.db");

		public void FillDb(int quantity)
		{
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
               
                StringBuilder steetsRequest = new StringBuilder();
                StringBuilder phoneNumbersRequest = new StringBuilder();
                StringBuilder adressesRequest = new StringBuilder();
                StringBuilder abonentsRequest = new StringBuilder();

                Random rand = new();
				for (int i = 6; i < quantity; i++)
				{
					//Заполняю таблицу Streets
					steetsRequest.Append($"INSERT INTO Streets (Name) VALUES ('Street{i}');");
					//Заполняю таблицу PhoneNumbers
					phoneNumbersRequest.Append($"INSERT INTO PhoneNumbers (Home, Work, Mobile) " +
                        $"VALUES ('{rand.Next(1, 21).ToString()}-" +
                        $"{rand.Next(1, 21).ToString()}-" +
                        $"{rand.Next(1, 21).ToString()}{i}', '{rand.Next(1, 21).ToString()}-" +
                        $"{rand.Next(1, 21).ToString()}-{rand.Next(1, 21).ToString()}{i}', " +
                        $"'{rand.Next(1, 21).ToString()}-{rand.Next(1, 21).ToString()}-" +
                        $"{rand.Next(1, 21).ToString()}{i}');");
					//Заполняю таблицу Adresses
					adressesRequest.Append($"INSERT INTO Adresses (Street_Id, House) " +
                        $"VALUES ({rand.Next(1, i).ToString()}, '{rand.Next(1, 21).ToString()}{i.ToString()}');");
					//Заполняю таблицу Abonents
					abonentsRequest.Append($"INSERT INTO Abonents (NameFio, Adress_Id, PhoneNumber_Id) " +
                        $"VALUES ('ФИО_{i.ToString()}', {i.ToString()}, {i.ToString()});");
				}

				try
				{
					connection.Open();
					SqliteCommand command = new SqliteCommand(steetsRequest.ToString(), connection);
					command.ExecuteNonQuery();
					command = new SqliteCommand(phoneNumbersRequest.ToString(), connection);
					command.ExecuteNonQuery();
					command = new SqliteCommand(adressesRequest.ToString(), connection);
					command.ExecuteNonQuery();
					command = new SqliteCommand(abonentsRequest.ToString(), connection);
					command.ExecuteNonQuery();
				}
				catch //(Exception ex)
				{
					
				}
				finally
				{
					connection.Close();
				}
			}
		}

        public IEnumerable<RequestAbonent> SelectAllAbonents()
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                IEnumerable<RequestAbonent>? result = null;
                try
                {
                    connection.Open();
                    result =
                        connection.Query<RequestAbonent>(@"
                            SELECT Ab.NameFio AS 'Fio', 
                                S.Name AS 'Street', 
                                Ad.House AS 'HouseNumber', 
                                Ph.Home AS 'HomePhone', 
                                Ph.Work AS 'WorkPhone', 
                                Ph.Mobile AS 'MobilePhone' 
                            FROM Abonents AS Ab 
                                JOIN Adresses AS Ad ON Ab.Adress_Id=Ad.Id 
                                JOIN PhoneNumbers AS Ph ON Ab.PhoneNumber_Id=Ph.Id 
                                JOIN Streets AS S ON Ad.Street_Id=S.Id; ");
                }
                catch //(Exception ex)
                {
                    
                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }

        public IEnumerable<StreetsAbonentsCount> SelectStreetsAbonentsCount()
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                IEnumerable<StreetsAbonentsCount>? result = null;
                try
                {
                    connection.Open();
                    result =
                        connection.Query<StreetsAbonentsCount>(@"
                            SELECT S.Name AS StreetName, COUNT(Ab.Id) AS AbonentsCount
                            FROM Streets AS S
                            JOIN Adresses AS Ad ON Ad.Street_Id
                            JOIN Abonents AS Ab ON Ab.Adress_Id
                            WHERE Ab.Adress_Id = Ad.Id AND Ad.Street_Id = S.Id
                            GROUP BY S.Name");
                }
                catch //(Exception ex)
                {
                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }
    }
}
