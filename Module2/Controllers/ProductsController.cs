using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module2.Data;
using Module2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Module2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ProductsDbContext _productsDbContext;

        public ProductsController(ProductsDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productsDbContext.Products;
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
                return NotFound("Product not found...");
            }

            // Remove the product
            _productsDbContext.Products.Remove(product);
            _productsDbContext.SaveChanges(true);
            return Ok("Product is removed");
        }
    }
}