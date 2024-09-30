using CRUDStoreProsedureDemo.Models;
using CRUDStoreProsedureDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IProductService, ProductService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var productGroup = app.MapGroup("/api/product");

productGroup.MapPost("", async (Product product, IProductService productService) =>
{
    await productService.InsertAsync(product);

    return TypedResults.Ok();
});

productGroup.MapPut("", async (Product product, IProductService productService) =>
{
    await productService.UpdateAsync(product);

    return TypedResults.NoContent();
});

productGroup.MapGet("", async (IProductService productService) =>
{
    var products = await productService.ListAsync();

    return TypedResults.Ok(products);
});

productGroup.MapGet("{id}", async (int id, IProductService productService) =>
{
    var product = await productService.GetAsync(id);

    return TypedResults.Ok(product);
});

productGroup.MapDelete("{id}", async (int id, IProductService productService) =>
{
    await productService.DeleteAsync(id);

    return TypedResults.Ok();
});

app.Run();
