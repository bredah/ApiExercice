using Microsoft.AspNetCore.Mvc;
using Module1.Models;
using System.Collections.Generic;

namespace Module1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        private static List<Product> _products = new List<Product>()
        {
            new Product(){ProductId = 1, ProductName="laptopt", ProductPrice="200"},
            new Product(){ProductId = 1, ProductName="tv", ProductPrice="400"},
        };

        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(_products);
        }

        [HttpPost]
        public void Post([FromBody]Product product)
        {
            _products.Add(product);
            //return StatusCode(StatusCode);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Product product)
        {
            foreach (Product item in _products.FindAll(x => x.ProductId == id))
            {
                item.ProductName = product.ProductName;
                item.ProductPrice = product.ProductPrice;
            }

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _products.RemoveAll(product => product.ProductId == id);
        }
    }
}