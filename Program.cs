using Microsoft.EntityFrameworkCore;
using WebApp.Models;

// https://learn.microsoft.com/en-gb/aspnet/core/security/cors?view=aspnetcore-7.0
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://example.com",
                                              "http://www.contoso.com");
                      });
});


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

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