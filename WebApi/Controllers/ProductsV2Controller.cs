using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly ProductsDbContext _productsDbContext;

        public ProductsV2Controller(ProductsDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Product> Get(string searchDescription = null, string sortPrice = null, int? pageNumber = 1, int? pageSize = 5)
        {
            int currentPage = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 1;
            IQueryable<Product> products;

            products = String.IsNullOrEmpty(searchDescription) ? _productsDbContext.Products : _productsDbContext.Products.Where(p => p.ProductName.Contains(searchDescription));

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

            return products.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var product = _productsDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found...");
            }

            return Ok(product);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productsDbContext.Products.Add(product);
            _productsDbContext.SaveChanges(true);
            return CreatedAtAction("Get", product);
        }




        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Check the id
            if (id != product.Id)
            {
                return BadRequest("Check the product id");
            }

            // Update the product
            try
            {
                _productsDbContext.Products.Update(product);
                _productsDbContext.SaveChanges(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("No record found against with id");
            }

            return AcceptedAtAction("Get", product);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Check if the product exists
            var product = _productsDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return BadRequest("Product not found...");
            }

            // Remove the product
            _productsDbContext.Products.Remove(product);
            _productsDbContext.SaveChanges(true);
            return Ok("Product is removed");
        }
    }
}