using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.Services
{
    public class ProductRepositoryTest : ProductRepositoryFixture
    {
        public async Task GetProducts()
        {

        }

        public async Task GetProduct()
        {

        }


        [Fact]
        public async Task AddProductAsync()
        {
            var product = new Product() { Id = 1, ProductName = "desktop", Price = "200.00" };
            // Run the test against one instance of the context
            using (var context = new ProductsDbContext(options))
            {
                var service = new ProductRepository(context);
                service.AddProduct(product);
            }
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ProductsDbContext(options))
            {
                Assert.Equal(4, await context.Products.CountAsync());
            }
        }
    }
}
