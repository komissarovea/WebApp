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
        public IEnumerable<Product> GetProducts(long? id)
        {
            var product = context.Products.Find(id);
            return product != null ? new[] { product } : context.Products;
        }

        [HttpGet("{id}")]
        public Product? GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            logger.LogDebug("GetProduct Action Invoked");
            return context.Products.Find(id);
            //return context.Products.FirstOrDefault();
        }

        [HttpPost]
        public void SaveProduct([FromBody] Product product)
        {
            // Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{ Name="SoccerBoots"; Price=89.99; CategoryId=2; SupplierId=2} | ConvertTo-Json) -ContentType "application/json"
            context.Products.Add(product);
            context.SaveChanges();
        }

        [HttpPut]
        public void UpdateProduct([FromBody] Product product)
        {
            // Invoke-RestMethod http://localhost:5000/api/products -Method PUT -Body (@{ ProductId=1; Name="Green Kayak"; Price=275; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
            context.Products.Update(product);
            context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(long id)
        {
            // Invoke-RestMethod http://localhost:5000/api/products/2 -Method DELETE
            context.Products.Remove(new Product() { ProductId = id });
            context.SaveChanges();
        }
    }
}