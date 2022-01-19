using FluentValidation;
using IWantApp.Domain.Products;
using IWantApp.Endpoints.Auth;
using IWantApp.Endpoints.Categories;
using IWantApp.Endpoints.Employees;
using IWantApp.Endpoints.Validators;
using IWantApp.Infra.Data;
using IWantApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database EF configuration
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("SqlServerConnectionString"));

// Identity configure
builder.Services.AddIdentity<IdentityUser, IdentityRole>(/*options =>
{
    
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
}*/).AddEntityFrameworkStores<ApplicationDbContext>();

// Authorization configure
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeePolicy", p => p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
    options.AddPolicy("Employee005Policy", p => p.RequireAuthenticatedUser().RequireClaim("EmployeeCode", "005"));
});

// Authentication configure
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});

// Swagger configure
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

// Dependency injection (DI)
builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
builder.Services.AddScoped<IJwTUtils, JwTUtils>();

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

app.UseAuthentication();
app.UseAuthorization();

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

app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle)
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle)
    .Produces<List<EmployeeResponse>>(StatusCodes.Status200OK);

app.MapMethods(EmployeeGet.Template, EmployeeGet.Methods, EmployeeGet.Handle)
    .Produces<EmployeeResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapMethods(EmployeePut.Template, EmployeePut.Methods, EmployeePut.Handle)
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces<Dictionary<string, string[]>>(StatusCodes.Status400BadRequest);

app.MapMethods(GenerateTokenPost.Template, GenerateTokenPost.Methods, GenerateTokenPost.Handle)
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.Run();