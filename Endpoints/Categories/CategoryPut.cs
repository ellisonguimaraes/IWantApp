using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute]Guid id, [FromBody]CategoryRequest categoryRequest, [FromServices]ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null) 
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, "Edited for route");

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
       
        context.Categories.Update(category);
        context.SaveChanges();

        return Results.Ok();
    }
}
