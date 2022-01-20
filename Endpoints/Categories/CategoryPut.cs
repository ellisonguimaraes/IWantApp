using FluentValidation;
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
    public static Delegate Handle => ActionAsync;

    [Authorize]
    public static async Task<IResult> ActionAsync([FromRoute] Guid id, 
                                 [FromBody] CategoryRequest categoryRequest, 
                                 [FromServices] ApplicationDbContext context,
                                 [FromServices] IValidator<Category> validator,
                                 HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null) 
            return Results.NotFound();
        
        category.Name = categoryRequest.Name;
        category.Active = categoryRequest.Active;
        category.EditedBy = userId;

        var result = validator.Validate(category);

        if (!result.IsValid)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
       
        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return Results.Ok(category.Id);
    }
}
