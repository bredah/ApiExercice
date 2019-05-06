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
        private static bool Seeded = false;

        public ProductsControllerFixture()
        {
            Seed();
        }

        public void Seed()
        {
            if (Seeded) { return; }


            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var dbContext = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                .Options;

            using (var context = new ProductsDbContext(dbContext))
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

            Seeded = true;
        }
    }
}