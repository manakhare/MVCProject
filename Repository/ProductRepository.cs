using System;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Practive_vs_1.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Practive_vs_1.Repository;

public class ProductRepository
{

    public static string connectionString = "server=localhost;port=3306;Database=products;uid=root;pwd=Mana@123";


    static MySqlConnection connection = new MySqlConnection(connectionString);

    public List<dynamic> GetAllProducts()
    {
        var users = connection.Query<dynamic>("SELECT * FROM products join images on products.id=images.id;").ToList();
        return users.ToList();
    }

    public List<dynamic> GetAllProducts(int? priceFrom, int? priceTo, string? brand, string? category)
    {
        string query = "select * from ProductDB";

        if (brand != null)
        {
            query = query + $" WHERE brand IN (\'{brand}\')";

            if (category != null)
            {
                query = query + $" AND category IN (\'{category}\')";
            }

            if (priceFrom != null && priceTo != null)
            {
                query = query + $" AND price BETWEEN {priceFrom} AND {priceTo}";
            }
        }
        else if (category != null)
        {
            query = query + $" WHERE category IN (\'{category}\')";
            if (priceFrom != null && priceTo != null)
            {
                query = query + $" AND price BETWEEN {priceFrom} AND {priceTo}";
            }
        }
        else if (priceFrom != null && priceTo != null)
        {
            query = query + $" AND price BETWEEN {priceFrom} AND {priceTo}";
        }
        else 
        {
            query = "SELECT * from products join images on products.id = images.id";
        }


        var final_products =  connection.Query<dynamic>(query);
        return final_products.ToList();
    }

}