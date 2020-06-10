using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace DataAccesLibrary
{
    public class SQLiteDataAcces
    {
        public List<T> LoadData<T, U>(string sqliteStatement, U parameters, string connectionString )
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqliteStatement, parameters).ToList();
                return rows;
            }

        }

        public void SaveData<T>(string sqliteStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqliteStatement, parameters);
            }
        }
    }
}
