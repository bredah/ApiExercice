using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v2
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct productRepository;

        public ProductsController(IProduct productRepository)
        {
            this.productRepository = productRepository;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get(string searchDescription = null, string sortPrice = null, int pageNumber = 1, int pageSize = 5)
        {
            return Ok(productRepository.GetProducts(searchDescription,sortPrice,pageNumber,pageSize));
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var product = productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound("Product not found");
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
            productRepository.AddProduct(product);
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
                productRepository.UpdateProduct(product);
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
            try
            {
                var count = productRepository.DeleteProduct(id);
                if (count < 1)
                {
                    return NotFound("No record found against with id");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("It not possible to remove the product with this id");
            }
            return Ok("Product is removed");
        }
    }
}