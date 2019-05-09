using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebApi.Models;

namespace WebApi.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            using (StreamReader reader = new StreamReader("data.json"))
            {
                var json = reader.ReadToEnd();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                modelBuilder.Entity<Product>().HasData(products);
            }
        }
    }
}