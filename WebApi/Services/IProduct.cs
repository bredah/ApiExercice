using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IProduct
    {
        IEnumerable<Product> GetProducts(string searchDescription = null, string sortPrice = null, int pageNumber = 1, int pageSize = 1);
        Product GetProduct(int id);
        void AddProduct(Product product);
        int UpdateProduct(Product product);
        int DeleteProduct(int id);
    }
}
