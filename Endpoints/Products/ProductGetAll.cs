using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action([FromServices] ApplicationDbContext context)
    {
        var products = context.Products
                            .Include(p => p.Category)
                            .Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.HasStock))
                            .ToList();

        return Results.Ok(products);
    }
}
