using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private DataContext context;

        public SuppliersController(DataContext ctx)
        {
            context = ctx;
        }

        // http://localhost:5000/api/suppliers/1
        [HttpGet("{id}")]
        public async Task<Supplier?> GetSupplier(long id)
        {
            Supplier supplier = await context.Suppliers.Include(s => s.Products).FirstAsync(s => s.SupplierId == id);
            if (supplier.Products != null)
            {
                foreach (Product p in supplier.Products)
                {
                    p.Supplier = null;
                };
            }
            return supplier;
        }

        // Invoke-RestMethod http://localhost:5000/api/suppliers/1 -Method PATCH -ContentType "application/json" -Body '[{"op":"replace","path":"City","value":"Los Angeles"}]'
        // https://www.rfc-editor.org/rfc/rfc6902#section-4.6
        [HttpPatch("{id}")]
        public async Task<Supplier?> PatchSupplier(long id, JsonPatchDocument<Supplier> patchDoc)
        {
            Supplier? s = await context.Suppliers.FindAsync(id);
            if (s != null)
            {
                patchDoc.ApplyTo(s);
                await context.SaveChangesAsync();
            }
            return s;
        }
    }
}
