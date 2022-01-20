using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync([FromBody] EmployeeRequest employeeRequest,
                                 [FromServices] UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser
        {
            UserName = employeeRequest.Email,
            Email = employeeRequest.Email
        };

        var result = await userManager.CreateAsync(user, employeeRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("Name", employeeRequest.Name),
            new Claim("EmployeeCode", employeeRequest.EmployeeCode)
        };

        var resultClaims = await userManager.AddClaimsAsync(user, userClaims);

        if (!result.Succeeded)
            return Results.ValidationProblem(resultClaims.Errors.ConvertToProblemDetails());

        return Results.Created($"{Template}/{user.Id}", user.Id);
    }
}
