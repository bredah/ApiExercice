using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Tests.Services
{
    public class ProductRepositoryFixture
    {

        protected readonly DbContextOptions<ProductsDbContext> options;

        protected readonly IList<Product> products;

        public ProductRepositoryFixture()
        {
            products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = "10.00" },
                new Product(){ Id = 2, ProductName = "Teclado", Price = "20.00" },
                new Product(){ Id = 3, ProductName = "Notebook", Price = "20.00" }
            };

            options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "bookstore")
                .Options;

            InitalState();
        }

        private void InitalState()
        {
            using (var context = new ProductsDbContext(options))
            {
                var service = new ProductRepository(context);
                service.AddProduct(products[0]);
            }
        }
    }
}
