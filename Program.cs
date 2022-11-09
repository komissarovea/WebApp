using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

const string BASEURL = "api/products";
var todoItems = app.MapGroup(BASEURL);

todoItems.MapGet("/{id}", async (long id, DataContext data) =>
{
    Product? p = await data.Products.FindAsync(id);
    return p == null ? Results.NotFound() : TypedResults.Ok(p);
});

todoItems.MapGet("/", GetAllProducts);

// Invoke-RestMethod http://localhost:5000/api/products -Method POST -Body (@{Name="Group Goggles"; Price=11.75; CategoryId=1; SupplierId=1} | ConvertTo-Json) -ContentType "application/json"
todoItems.MapPost("/", async (Product p, DataContext data) =>
{
    await data.AddAsync(p);
    await data.SaveChangesAsync();
    return TypedResults.Created($"/todoitems/{p.ProductId}", p);
});

app.UseMiddleware<WebApp.TestMiddleware>();

app.MapGet("/", () =>
{
    return "Hello World2!";
});

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();

static async Task<IResult> GetAllProducts(DataContext db)
{
    return TypedResults.Ok(await db.Products.ToArrayAsync());
}

// app.Use(async (HttpContext context, Func<Task> next) =>
// {
//     //await Task.CompletedTask;
//     await next();
// });

//Action<Task> a;
//Predicate<Task> p;
// app.Use(async (HttpContext context, RequestDelegate next) =>
// {
//     await next(context);
// });
// app.Run((HttpContext context) =>
// {
//     return Task.CompletedTask;
// });