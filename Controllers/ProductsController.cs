using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
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
        public async Task<Product?> GetProduct(long id)
        {
            return await context.Products.FindAsync(id);
        }

        // Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{ Name="SoccerBoots"; Price=89.99; CategoryId=2; SupplierId=2} | ConvertTo-Json) -ContentType "application/json"
        // Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{ProductId=100; Name="Swim Buoy"; Price=19.99; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
        [HttpPost]
        public async Task SaveProduct([FromBody] ProductBindingTarget target)
        {
            await context.Products.AddAsync(target.ToProduct());
            await context.SaveChangesAsync();
        }


        // Invoke-RestMethod http://localhost:5000/api/products -Method PUT -Body (@{ ProductId=1; Name="Green Kayak"; Price=275; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
        [HttpPut]
        public async Task UpdateProduct([FromBody] Product product)
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
    }
}