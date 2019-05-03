using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Integration.Tests.Controller
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
                using (StreamReader reader = new StreamReader("data.json"))
                {
                    var json = reader.ReadToEnd();
                    var products = JsonConvert.DeserializeObject<List<Product>>(json);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                    context.Database.CloseConnection();
                }
            }
        }

        private DbContextOptions<ProductsDbContext> GetDbContextOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            return new DbContextOptionsBuilder<ProductsDbContext>()
                .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }
    }
}