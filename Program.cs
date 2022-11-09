using Microsoft.EntityFrameworkCore;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () =>
{
    return "Hello World!";
});

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);

app.Run();

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