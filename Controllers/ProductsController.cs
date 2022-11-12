using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private DataContext context;

        public ProductsController(DataContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return context.Products.AsAsyncEnumerable();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            Product? p = await context.Products.FindAsync(id);
            if (p == null)
            {
                return NoContent();
            }
            return Ok(p);
        }

        // Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{ Name="SoccerBoots"; Price=89.99; CategoryId=2; SupplierId=2} | ConvertTo-Json) -ContentType "application/json"
        // Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{ProductId=100; Name="Swim Buoy"; Price=19.99; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
        // Invoke-WebRequest http://localhost:5000/api/products -Method POST -Body (@{Name="BootLaces"} | ConvertTo-Json) -ContentType "application/json"
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductBindingTarget target) // [FromBody] not needed in api controller
        {
            // if (ModelState.IsValid)
            // {
            Product p = target.ToProduct();
            await context.Products.AddAsync(p);
            await context.SaveChangesAsync();
            return Created($"/api/products/{p.ProductId}", p);
            // }
            // return BadRequest(ModelState);
        }


        // Invoke-RestMethod http://localhost:5000/api/products -Method PUT -Body (@{ ProductId=1; Name="Green Kayak"; Price=275; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
        [HttpPut]
        public async Task UpdateProduct(Product product) // [FromBody] not needed in api controller
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }

        // Invoke-RestMethod http://localhost:5000/api/products/2 -Method DELETE
        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            context.Products.Remove(new Product() { ProductId = id });
            await context.SaveChangesAsync();
        }

        // http://localhost:5000/api/products/redirect
        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            return RedirectToRoute(new
            {
                controller = "Products",
                action = "GetProduct",
                Id = 3
            });
            // return RedirectToAction(nameof(GetProduct), new { Id = 3 });
            // return Redirect("/api/products/1");
            //  return LocalRedirect("http://google.com"); // exception!!!
        }
    }
}