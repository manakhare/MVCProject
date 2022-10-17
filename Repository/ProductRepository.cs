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

    public static string connectionString = "server=localhost;port=3306;Database=products;uid=root;pwd=ChoosePassword";
    static MySqlConnection connection = new MySqlConnection(connectionString);

    public string CreateJointString(string filter)
    {
        String[] arr = filter.Split(",");
        for (int i = 0; i < arr.Count(); i++)
        {
            arr[i] = "\"" + arr[i] + "\"";
        }

        string filters = string.Join(", ", arr);
        return filters;
    }

    public string CreateQuery(int? priceFrom, int? priceTo, string? brand, string? category)
    {
        string query = "SELECT *, (SELECT GROUP_CONCAT(images.image_link SEPARATOR ', ') FROM images WHERE producttable.id = images.image_id) as imageList from producttable";
        Console.WriteLine(query);

        if (brand == null && category == null && priceFrom == null && priceTo == null)
        {
            return query;
        }
    
        else
        {
            query += " WHERE";
        }

        if (category != null)
        {
            query = query + $" category IN ({category})";
            
        }

        if (brand != null)
        {
            if (category != null) query += " AND";
            query = query + $" brand IN ({brand})";
        }

        if (priceFrom != null && priceTo != null)
        {
            if (brand != null || category != null) query += " AND";
            query = query + $" WHERE price BETWEEN {priceFrom} AND {priceTo}";
        }

        return query;
    }


    public List<ProductModel> GetAllProducts(int? priceFrom, int? priceTo, string? brand, string? category)
    {
        if (brand != null) brand = CreateJointString(brand);
        if (category != null) category = CreateJointString(category);

        string query = CreateQuery(priceFrom, priceTo, brand, category);

        var final_product_data = connection.Query<ProductModel>(query);
        var final_products = final_product_data.Select((obj, i) => new ProductModel
        {
            id = obj.id,
            title = obj.title,
            description = obj.description,
            price = obj.price,
            discountPercentage = obj.discountPercentage,
            rating = obj.rating,
            stock = obj.stock,
            brand = obj.brand,
            category = obj.category,
            thumbnail = obj.thumbnail,
            imageList = obj.imageList.Contains(",") ? obj.imageList.Split(", ") : obj.imageList
        }).ToList<ProductModel>();

        return final_products;
    }

}