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
        [HttpGet("object")]
        public async Task<Product> GetObject()
        {
            return await context.Products.FirstAsync();
        }
    }
}