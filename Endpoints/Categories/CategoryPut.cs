using FluentValidation;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute]Guid id, 
                                 [FromBody]CategoryRequest categoryRequest, 
                                 [FromServices]ApplicationDbContext context,
                                 [FromServices]IValidator<Category> validator)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null) 
            return Results.NotFound();
        
        category.Name = categoryRequest.Name;
        category.Active = categoryRequest.Active;
        category.EditedBy = "Edited for route";

        var result = validator.Validate(category);

        if (!result.IsValid)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
       
        context.Categories.Update(category);
        context.SaveChanges();

        return Results.Ok();
    }
}
