using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.Services
{
    public class ProductRepositoryTest : IDisposable
    {
        private IList<Product> _products;
        private readonly DbContextOptions<ProductsDbContext> _options;
        private readonly ProductsDbContext _context;


        public ProductRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "bookstore")
                .Options;

            _context = new ProductsDbContext(_options);

            Seed();
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void GetProducts()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts();
                Assert.Equal(_products.Count, result.Count());
            }
        }

        [Fact]
        public void GetProducts_SearchDescription()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts(searchDescription: "bo");
                Assert.Single(result);
            }
        }

        [Fact]
        public void GetProducts_SearchDescription_NotExist()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts(searchDescription: "zz");
                Assert.Empty(result);
            }
        }

        [Fact]
        public void GetProducts_PageSize()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts(pageSize: 3);
                Assert.Equal(3, result.Count());
            }
        }

        [Fact]
        public void GetProducts_SortPrice_Asc()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts(sortPrice: "asc");
                Assert.True(result.ElementAt(0).Price < result.ElementAt(1).Price);
            }
        }

        [Fact]
        public void GetProducts_SortPrice_Desc()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProducts(sortPrice: "desc");
                Assert.True(result.ElementAt(0).Price > result.ElementAt(1).Price);
            }
        }

        [Fact]
        public void GetProduct()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.GetProduct(_products[0].Id);
                Assert.IsType<Product>(result);
                Assert.Equal(_products[0], result);
            }
        }

        [Fact]
        public void AddProduct()
        {
            // Run the test against one instance of the context
            using (var repository = new ProductRepository(new ProductsDbContext(_options)))
            {
                repository.AddProduct(new Product() { ProductName = "desktop", Price = 150.00M });                
            }
            // Use a separate instance of the context to verify correct data was saved to database
            using (var repository = new ProductRepository(new ProductsDbContext(_options)))
            {
                Assert.Equal(_products.Count + 1, repository.GetProducts().Count());
            }
        }

        [Fact]
        public void Update()
        {
            var product = _products[0].ShallowCopy();
            product.ProductName = "New Mouse";
            // Run the test against one instance of the context
            using (var repository = new ProductRepository(new ProductsDbContext(_options)))
            {
                var result = repository.UpdateProduct(product);
                Assert.Equal(1, result);
            }
            // Use a separate instance of the context to verify correct data was saved to database
            using (var repository = new ProductRepository(new ProductsDbContext(_options)))
            {
                var result = repository.GetProduct(product.Id);
                Assert.IsType<Product>(result);
                Assert.Equal(product, result);
                Assert.NotEqual(_products[0], result);
            }
        }

        [Fact]
        public void DeleteProduct()
        {
            using (var repository = new ProductRepository(_context))
            {
                var result = repository.DeleteProduct(_products[0].Id);
                Assert.True(result > 0);
            }
        }

        private void Seed()
        {
            _products = new List<Product>()
            {
                new Product() { ProductName = "Mouse", Price = 10.00M },
                new Product() { ProductName = "Keyboard", Price = 15.00M },
                new Product() { ProductName = "Gamepad", Price = 25.00M },
            };

            using (var context = new ProductsDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Products.AddRange(_products);
                context.SaveChanges();
            }
        }        
    }
}
