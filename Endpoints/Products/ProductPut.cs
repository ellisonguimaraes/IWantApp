using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Products;

public class ProductPut
{
    public static string Template => "/products/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync(
        [FromServices] ApplicationDbContext context,
        [FromServices] UserManager<IdentityUser> userManager,
        [FromRoute] Guid id,
        [FromBody] ProductRequest productRequest,
        HttpContext httpContext)
    {
        var product = context.Products.Where(p => p.Id.Equals(id)).SingleOrDefault();

        if (product == null)
            return Results.NotFound();

        var category = context.Categories.Where(c => c.Id.Equals(productRequest.CategoryId)).SingleOrDefault();

        if (category == null)
            return Results.BadRequest("Categoria não existe");

        var userId = httpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Single().Value;

        product.Name = productRequest.Name;
        product.Description = productRequest.Description;
        product.HasStock = productRequest.HasStock;
        product.CategoryId = category.Id;
        product.EditedBy = userId;
        product.EditedOn = DateTime.Now;

        context.Products.Update(product);
        await context.SaveChangesAsync();

        return Results.Ok(product.Id);
    }
}
