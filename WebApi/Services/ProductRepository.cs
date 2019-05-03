using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public class ProductRepository : IProduct, IDisposable
    {
        private readonly ProductsDbContext DBContext;

        public ProductRepository(ProductsDbContext DBContext)
        {
            this.DBContext = DBContext;
        }

        public void Dispose()
        {
            this.DBContext.Dispose();
        }

        public void AddProduct(Product product)
        {
            DBContext.Products.Add(product);
            DBContext.SaveChanges(true);
        }

        public int DeleteProduct(int id)
        {
            var product = DBContext.Products.Find(id);
            DBContext.Products.Remove(product);
            return DBContext.SaveChanges(true);
        }



        public Product GetProduct(int id)
        {
            var product = DBContext.Products.SingleOrDefault(p => p.Id == id);
            return product;
        }

        public IEnumerable<Product> GetProducts(string searchDescription = null, string sortPrice = null, int pageNumber = 1, int pageSize = 1)
        {
            IQueryable<Product> products;

            products = String.IsNullOrEmpty(searchDescription) ? DBContext.Products : DBContext.Products.Where(p => p.ProductName.Contains(searchDescription));

            switch (sortPrice)
            {
                case "asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    break;

            }
            // Return all products without page
            if (pageSize == 1)
            {
                return products;
            }
            return products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int UpdateProduct(Product product)
        {
            var entity = DBContext.Products.Find(product.Id);
            DBContext.Entry(entity).CurrentValues.SetValues(product);
            return DBContext.SaveChanges(true);
        }
    }
}
