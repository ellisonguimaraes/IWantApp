using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Products;

public class ProductDelete
{
    public static string Template => "/products/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync([FromServices] ApplicationDbContext context, [FromRoute] Guid id)
    {
        var user = context.Products.Where(p => p.Id.Equals(id)).SingleOrDefault();

        if (user == null)
            return Results.NotFound();

        context.Products.Remove(user);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
