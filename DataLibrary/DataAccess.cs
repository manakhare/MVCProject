using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using System.Linq;

namespace Practive_vs_1.DataLibrary;

public class DataAccess
{
    public List<T> LoadData<T, U>(string sql, U parameters, string connectionString)
    {
        using (IDbConnection connection = new MySqlConnection(connectionString))
        {
            //Each row that comes our of the query, will be mapped to the model T.
            List<T> rows = connection.Query<T>(sql, parameters).ToList();
            
            return rows;
        }
    }  
    public void SaveData<T>(string sql, T parameters, string connectionString)
    {
        using (IDbConnection connection = new MySqlConnection(connectionString))
        {
            //Each row that comes our of the query, will be mapped to the model T.
            connection.Execute(sql, parameters);
        }
    }  
}