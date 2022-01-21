using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Endpoints.Products;

public class ProductGet
{
    public static string Template => "/products/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    
    [AllowAnonymous]
    public static IResult Action([FromServices] ApplicationDbContext context, [FromRoute] Guid id)
    {
        var product = context.Products
                        .Include(p => p.Category)
                        .Where(p => p.Id.Equals(id))
                        .SingleOrDefault();

        if (product == null)
            return Results.NotFound();

        return Results.Ok(new ProductResponse(product.Id, product.Name, product.Description, product.HasStock));
    }
}
