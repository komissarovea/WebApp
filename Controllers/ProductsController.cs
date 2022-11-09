using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {

        [AcceptVerbs]
        public IEnumerable<Product> GetProducts()
        {
            return new Product[]
            {
                new Product() { Name = "Product #1" },
                new Product() { Name = "Product #2" },
            };
        }

        [HttpGet("{id}")]
        public Product GetProduct([FromRoute] long id)
        {
            return new Product()
            {
                ProductId = id,
                Name = "Test Product"
            };
        }
    }
}