using System.Diagnostics;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Practive_vs_1.Models;
using Practive_vs_1.Repository;
using PagedList;

namespace Practive_vs_1.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _productRepository;
    private static int LIMIT=5;
    public ProductController()
    {
        _productRepository = new ProductRepository();
    }

    public IPagedList<dynamic> Index(int? priceFrom, int priceTo, string? brand, string? category, int page=1)
    {
        var data = _productRepository.GetAllProducts(priceFrom, priceTo, brand, category);
        return data.ToPagedList(page, LIMIT);
    }
}