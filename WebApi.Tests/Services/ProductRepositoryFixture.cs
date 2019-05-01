using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Tests.Services
{
    public class ProductRepositoryFixture
    {

        protected readonly DbContextOptions<ProductsDbContext> Options;

        private readonly IList<Product> _products;

        protected ProductRepositoryFixture()
        {
            _products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = "10.00" },
                new Product(){ Id = 2, ProductName = "Keyboard", Price = "20.00" },
                new Product(){ Id = 3, ProductName = "Notebook", Price = "200.00" }
            };

            Options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "bookstore")
                .Options;

            InitialState();
        }

        private void InitialState()
        {
            using (var context = new ProductsDbContext(Options))
            {
                var service = new ProductRepository(context);
                service.AddProduct(_products[0]);
            }
        }
    }
}
