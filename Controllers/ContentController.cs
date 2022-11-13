using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase
    {
        private DataContext context;

        public ContentController(DataContext dataContext)
        {
            context = dataContext;
        }

        // Invoke-WebRequest http://localhost:5000/api/content/string | select @{n='Content-Type';e={$_.Headers."Content-Type" }}, Content
        [HttpGet("string")]
        public string GetString() => "This is a string response";

        // Invoke-WebRequest http://localhost:5000/api/content/object | select @{n='Content-Type';e={$_.Headers."Content-Type" }}, Content
        // Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/xml"} | select @{n='Content-Type';e={ $_.Headers."Content-Type" }}, Content
        // 
        [HttpGet("object0")]
        public async Task<Product> GetObject0()
        {
            return await context.Products.FirstAsync();
        }

        [HttpGet("object")]
        public async Task<ProductBindingTarget> GetObject()
        {
            Product p = await context.Products.FirstAsync();
            return new ProductBindingTarget()
            {
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId
            };
        }

    }
}