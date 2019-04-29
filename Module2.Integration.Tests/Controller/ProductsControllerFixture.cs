using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Module2.Data;
using Module2.Models;
using Xunit;

namespace Module2.Integration.Tests.Controller
{
    public class ProductsControllerFixture
    {
        public ProductsControllerFixture()
        {
            using (var context = new ProductsDbContext(GetDbContextOptions()))
            {
                // 
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Add a new product
                var products = new List<Product>();
                products.Add(new Product() {Id = 1, ProductName = "First Product", Price = "100,00"});
                products.Add(new Product() {Id = 2, ProductName = "Second Product", Price = "200,00"});
                products.Add(new Product() {Id = 3, ProductName = "Old Product", Price = "300,00"});
                context.Products.AddRange(products);
                context.SaveChanges();
                context.Database.CloseConnection();
            }
        }

        private DbContextOptions<ProductsDbContext> GetDbContextOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            return new DbContextOptionsBuilder<ProductsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }
    }
}