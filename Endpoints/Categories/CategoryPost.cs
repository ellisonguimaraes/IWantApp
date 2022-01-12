using FluentValidation;
using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromBody] CategoryRequest categoryRequest, 
                                 [FromServices] ApplicationDbContext context, 
                                 [FromServices] IValidator<Category> validator)
    {
        var category = new Category(categoryRequest.Name, "Post Created Test", "Post Edited Test");

        var result = validator.Validate(category);

        if (!result.IsValid)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"{Template}/{category.Id}", category.Id);
    }
}
