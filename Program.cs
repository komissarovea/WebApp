using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApp.Models;

// https://learn.microsoft.com/en-gb/aspnet/core/security/cors?view=aspnetcore-7.0
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
{
    opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});

builder.Services.Configure<MvcOptions>(opts =>
{
    opts.RespectBrowserAcceptHeader = true;
    opts.ReturnHttpNotAcceptable = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://example.com",
                                              "http://www.contoso.com");
                      });
});

builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var app = builder.Build();

ThreadPool.GetMaxThreads(out int wt, out int cpt);
System.Console.WriteLine($"MaxThreads {wt}, {cpt}, {ThreadPool.CompletedWorkItemCount}");
ThreadPool.GetMinThreads(out wt, out cpt);
System.Console.WriteLine($"GetMinThreads {wt}, {cpt}, {ThreadPool.PendingWorkItemCount}");
ThreadPool.GetAvailableThreads(out wt, out cpt);
System.Console.WriteLine($"GetAvailableThreads {wt}, {cpt}, {ThreadPool.ThreadCount}");

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
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