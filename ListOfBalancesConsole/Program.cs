using System;
using Microsoft.Data.Sqlite;

namespace ListOfBalancesConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string sqlExpressionCreate = "INSERT INTO Data (id, Time, Balance) VALUES ('001ac','28.01.2022-1600', 10), ('001aa','28.01.2022-1601', 324647), ('001ab','28.01.2022-1602', 77777)";
            string sqlExpressionSearch;

            string time = SearchingDate();


            sqlExpressionSearch = SearchForBalance(time);

            using (var connection = new SqliteConnection("Data Source=BalanceHistory.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpressionCreate, connection);
                command.CommandText = "DROP TABLE IF EXISTS Data"; // Удаляем таблицу, обходной путь пока не разберемся с HasRows
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE Data (id TEXT NOT NULL PRIMARY KEY UNIQUE, Time TEXT NOT NULL UNIQUE, Balance INTEGER NOT NULL)";
                command.ExecuteNonQuery();
                command.CommandText = sqlExpressionCreate;
                command.ExecuteNonQuery();

                SqliteCommand commandTest = new SqliteCommand(sqlExpressionSearch, connection);       
                using (SqliteDataReader reader = commandTest.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetValue(0);
                            var date = reader.GetValue(1);
                            var balance = reader.GetValue(2);

                            Console.WriteLine($"{id} \t {date} \t {balance}");
                        }
                    }

                }        
                Console.Read();
            }
        }

        private static string SearchingDate()
        {
            Console.Write("Введите дату *например 28.01.2022*:");
            string? time = Console.ReadLine();
            return time;
        }

        private static string SearchForBalance(string time)
        {
            if (time == "")
            {
                SearchingDate();
            }
            time = time +'%';
            //string sqlExpressionSearch = "SELECT * FROM Data";

            string sqlExpressionSearch = $"SELECT * FROM Data WHERE Time LIKE '{time}'";
            return sqlExpressionSearch;
        }
    }
}
