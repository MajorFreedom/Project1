using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SuperProject
{
    class DataBaseConnector
    {
        public static SQLiteConnection connection = new SQLiteConnection(@"Data Source="+Path.Combine(Environment.CurrentDirectory, "DataBase.db")+"; Version=3;");
        SQLiteCommand command;
        SQLiteDataReader refer;

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        public void openConnection()
        {
            if(!File.Exists(Path.Combine(Environment.CurrentDirectory, "DataBase.db")))
            {
                SQLiteConnection.CreateFile(Path.Combine(Environment.CurrentDirectory, "DataBase.db"));
            }

            if(connection.State==System.Data.ConnectionState.Closed)
            {
                
                connection.Open();

                string sqlcommand = "CREATE TABLE IF NOT EXISTS table1 (" +
                    "Url MEDIUMTEXT," +
                    "Author MEDIUMTEXT," +
                    "Date MEDIUMTEXT," +
                    "Tags MEDIUMTEXT," +
                    "Header MEDIUMTEXT," +
                    "Introduction LONGTEXT," +
                    " Arcticle LONGTEXT);";
                command = new SQLiteCommand(sqlcommand, connection);
                command.ExecuteNonQuery();

            }
        }
        /// <summary>
        /// При вызове функции данные загружаются в таблицу базы данных
        /// </summary>
        /// <param name="data"> - информация, которая заносится в базу данных</param>
        public void Insert(List<string> data)
        {
            string sqlcommand = "INSERT INTO table1 " +
             "VALUES ('" + data[0] + "','" + data[1] + "','" + data[2] + "','"
                + data[3] + "','" + data[4] + "','" + data[5] + "','" + data[6] + "');";

            
            
            command = new SQLiteCommand(sqlcommand, connection);
            command.ExecuteNonQuery();

        }
        /// <summary>
        /// Проверка на наличии статьи в базе данных
        /// </summary>
        /// <param name="url"> - ссылка, которую нужно найти  в базе данных</param>
        /// <returns>Возращает true, если ссылки нет в базе данных</returns>
        public bool inDB(string url)
        {
            bool isEmpty = false; 
            string sqlcommand = "SELECT Url FROM table1 WHERE Url = '" + url + "';";
            command = new SQLiteCommand(sqlcommand, connection);
            refer = command.ExecuteReader();
            
           
            if (!refer.Read())
                isEmpty = true;

            refer.Close();
            return isEmpty;
        }
        /// <summary>
        /// При вызове функции закрывается соединение с базой данных
        /// </summary>
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        

    }
}
