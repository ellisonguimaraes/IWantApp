using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryDelete
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize]
    public static async Task<IResult> ActionAsync([FromRoute] Guid id, [FromServices] ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
            return Results.NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
