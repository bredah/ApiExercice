using Microsoft.AspNetCore.Mvc;
using Module1.Models;
using System.Collections.Generic;

namespace Module1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        static List<Customer> _customers = new List<Customer>()
        {
            new Customer(){Id= 1, Name = "X 01", Email = "x01@mail.com" , Phone = "123456"},
            new Customer(){Id= 1, Name = "X 02", Email = "x02@mail.com" , Phone = "456789"},
        };

        public IEnumerable<Customer> Get()
        {
            return _customers;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customers.Add(customer);
                return Ok();
            }
            return BadRequest(ModelState);
        }


    }
}