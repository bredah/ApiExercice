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

            Seed(_context);
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
                Assert.Equal(2, result.Count());
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
            var product = new Product() { ProductName = "desktop", Price = 150.00M };
            // Run the test against one instance of the context
            using (var repository = new ProductRepository(new ProductsDbContext(_options)))
            {
                repository.AddProduct(product);                
            }
            // Use a separate instance of the context to verify correct data was saved to database
            using (var repository = new ProductRepository(_context))
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
                Assert.IsType<Product>(product);
                Assert.IsType<Product>(result);
                Assert.NotEqual(product, result);
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

        private void Seed(ProductsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            _products = new List<Product>()
            {
                new Product(){ Id = 1, ProductName = "Mouse",Price = 10.00M },
                new Product(){ Id = 2, ProductName = "Keyboard", Price = 20.00M },
                new Product(){ Id = 3, ProductName = "Notebook", Price = 200.00M }
            };
            context.Products.AddRange(_products);
            context.SaveChanges(true);
        }        
    }
}
