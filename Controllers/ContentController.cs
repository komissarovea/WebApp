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
        // Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/xml,*/*;q=0.8"} | select @{n='Content-Type';e={ $_.Headers."Content-Type" }}, Content
        // Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="img/png"} | select @{n='Content-Type';e={ $_.Headers."Content-Type" }}, Content
        [HttpGet("object0")]
        public async Task<Product> GetObject0()
        {
            return await context.Products.FirstAsync();
        }

        // Invoke-WebRequest http://localhost:5000/api/content/object -Headers @{Accept="application/xml,application/json;q=0.8"} | select @{n='Content-Type';e={ $_.Headers."Content-Type"}}, Content
        // http://localhost:5000/api/content/object/json
        // http://localhost:5000/api/content/object/xml
        [HttpGet("object/{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
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

        // Invoke-RestMethod http://localhost:5000/api/content -Method POST -Body (@{ Name="Swimming Goggles"; Price=12.75; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
        [HttpPost]
        [Consumes("application/json", "application/xml")]
        public string SaveProductJson(ProductBindingTarget product)
        {
            return $"JSON: {product.Name}";
        }
        
        // Invoke-RestMethod http://localhost:5000/api/content -Method POST -Body "<ProductBindingTarget xmlns=`"http://schemas.datacontract.org/2004/07/WebApp.Models`"><CategoryId>1</CategoryId><Name>Kayak</Name><Price>275.00</Price><SupplierId>1</SupplierId></ProductBindingTarget>" -ContentType "application/xml"
        // [HttpPost]
        // [Consumes("application/xml")]
        // public string SaveProductXml(ProductBindingTarget product)
        // {
        //     return $"XML: {product.Name}";
        // }

    }
}