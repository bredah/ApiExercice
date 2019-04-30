using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public class ProductRepository : IProduct
    {
        private readonly ProductsDbContext productsDbContext;

        public ProductRepository(ProductsDbContext productsDbContext)
        {
            this.productsDbContext = productsDbContext;
        }

        public void AddProduct(Product product)
        {
            productsDbContext.Products.Add(product);
            productsDbContext.SaveChanges(true);
        }

        public void DeleteProduct(int id)
        {
            var product = productsDbContext.Products.Find(id);
            productsDbContext.Products.Remove(product);
            productsDbContext.SaveChanges(true);
        }

        public Product GetProduct(int id)
        {
            var product = productsDbContext.Products.SingleOrDefault(p => p.Id == id);
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return productsDbContext.Products;
        }

        public void UpdateProduct(Product product)
        {
            productsDbContext.Products.Update(product);
            productsDbContext.SaveChanges(true);
        }
    }
}
