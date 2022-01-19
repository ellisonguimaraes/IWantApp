using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePut
{
    public static string Template => "/employees/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee005Policy")]
    public static IResult Action([FromServices] UserManager<IdentityUser> userManager,
                                 [FromBody] EmployeeRequest employeeRequest,
                                 [FromRoute] string id)
    {
        var user = userManager.FindByIdAsync(id).Result;

        if (user == null)
            return Results.NotFound();

        var claims = userManager.GetClaimsAsync(user).Result;

        if (claims.Count <= 0)
        {
            return Results.BadRequest();
        }

        var claimName = claims.Where(c => c.Type == "Name").SingleOrDefault();

        if (claimName == null) return Results.BadRequest();

        var claimCode = claims.Where(c => c.Type == "EmployeeCode").SingleOrDefault();

        if (claimCode == null) return Results.BadRequest();

        user.UserName = employeeRequest.Email;
        user.Email = employeeRequest.Email;
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, employeeRequest.Password);

        var result = userManager.ReplaceClaimAsync(user, claimName, new Claim("Name", employeeRequest.Name)).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        result = userManager.ReplaceClaimAsync(user, claimCode, new Claim("EmployeeCode", employeeRequest.EmployeeCode)).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        result = userManager.UpdateAsync(user).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Ok();
    }
}
