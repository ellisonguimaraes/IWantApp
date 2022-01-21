using FluentValidation;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Products;

public class ProductPost
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync(
        [FromServices] ApplicationDbContext context, 
        [FromServices] IValidator<Product> validator,
        [FromBody] ProductRequest productRequest,
        HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Single().Value;

        var category = context.Categories.Where(c => c.Id.Equals(productRequest.CategoryId)).SingleOrDefault();

        if (category == null)
            return Results.BadRequest("Esse categoryid não existe");

        var product = new Product(productRequest.Name, productRequest.Description, productRequest.HasStock, productRequest.CategoryId, userId, userId);

        var result = validator.Validate(product);

        if (!result.IsValid)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"{Template}/{product.Id}", product.Id);
    }
}
