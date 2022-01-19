using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static IResult Action([FromRoute]Guid id, [FromBody]CategoryRequest categoryRequest, [FromServices]ApplicationDbContext context, HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null) 
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
       
        context.Categories.Update(category);
        context.SaveChanges();

        return Results.Ok();
    }
}
