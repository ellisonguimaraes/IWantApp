using FluentValidation;
using IWantApp.Domain.Products;
using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Validators;
using IWantApp.Infra.Data;
using Microsoft.OpenApi.Models;;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("SqlServerConnectionString"));

builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Minimal Api with .NET 6",
            Version = "v1",
            Description = "Base project Minimal Api's .Net 6",
            Contact = new OpenApiContact
            {
                Name = "Ellison Guimarães",
                Email = "ellison.guimaraes@gmail.com",
                Url = new Uri("https://github.com/ellisonguimaraes")
            }
        });
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = String.Empty;
        c.SwaggerEndpoint("swagger/v1/swagger.json", "Minimal API");
    });
}

app.UseHttpsRedirection();

app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle)
    .Produces<List<CategoryResponse>>(StatusCodes.Status200OK);

app.MapMethods(CategoryGet.Template, CategoryGet.Methods, CategoryGet.Handle)
    .Produces<CategoryResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle)
    .Produces<Guid>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle)
    .Produces<Guid>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces<Dictionary<string, string[]>>(StatusCodes.Status400BadRequest);

app.MapMethods(CategoryDelete.Template, CategoryDelete.Methods, CategoryDelete.Handle)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();