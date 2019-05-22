using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services {
    public class ProductRepository : IProduct, IDisposable {
        private readonly ProductsDbContext DBContext;

        public ProductRepository (ProductsDbContext DBContext) {
            this.DBContext = DBContext;
        }

        public void Dispose () {
            DBContext.Dispose ();
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// Add the new product
        /// </summary>
        /// <param name="product">Product to add</param>
        public int AddProduct (Product product) {
            DBContext.Products.Add (product);
            return DBContext.SaveChanges (true);
        }

        /// <summary>
        /// Delete the product from db
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>How many changes occurred during the delete</returns>
        public int DeleteProduct (int id) {
            var product = GetProduct(id);
            DBContext.Products.Remove(product);
            return DBContext.SaveChanges (true);
        }

        public Product GetProduct (int id) {
            var product = DBContext.Products.SingleOrDefault (p => p.Id == id);
            return product;
        }

        /// <summary>
        /// Retrive all product in the db
        /// </summary>
        /// <param name="searchDescription">Find a product by a short description</param>
        /// <param name="sortPrice">Sort the result by ASC or DESC</param>
        /// <param name="pageNumber">Show the result of a specific page</param>
        /// <param name="pageSize">How many items will be shown in the list</param>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts (string searchDescription = null, string sortPrice = null, int pageNumber = 1, int pageSize = 1) {
            IQueryable<Product> products;

            products = String.IsNullOrEmpty (searchDescription) ? DBContext.Products : DBContext.Products.Where (p => p.ProductName.Contains (searchDescription));

            switch (sortPrice) {
                case "asc":
                    products = products.OrderBy (p => p.Price);
                    break;
                case "desc":
                    products = products.OrderByDescending (p => p.Price);
                    break;
                default:
                    break;

            }
            // Return all products without page
            if (pageSize == 1) {
                return products;
            }
            return products.Skip ((pageNumber - 1) * pageSize).Take (pageSize).ToList ();
        }

        /// <summary>
        /// Update the product
        /// </summary>
        /// <param name="product">Product to update</param>
        /// <returns>How many changes occurred during the update</returns>
        public int UpdateProduct (Product product) {
            var entity = DBContext.Products.Find (product.Id);
            DBContext.Entry (entity).CurrentValues.SetValues (product);
            return DBContext.SaveChanges (true);
        }
    }
}