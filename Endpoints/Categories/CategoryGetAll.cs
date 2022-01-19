using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action([FromServices] ApplicationDbContext context)
    {
        var categories = context.Categories
                            .Select(c => new CategoryResponse(c.Id, c.Name, c.Active))
                            .ToList();

        return Results.Ok(categories);
    }
}
