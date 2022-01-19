using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories
                            .Select(c => new CategoryResponse { Id = c.Id, Name = c.Name, Active = c.Active })
                            .ToList();

        return Results.Ok(categories);
    }
}
