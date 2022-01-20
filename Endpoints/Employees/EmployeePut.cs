using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePut
{
    public static string Template => "/employees/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => ActionAsync;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> ActionAsync([FromServices] UserManager<IdentityUser> userManager,
                                 [FromBody] EmployeeRequest employeeRequest,
                                 [FromRoute] string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return Results.NotFound();

        var claims = await userManager.GetClaimsAsync(user);

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

        var result = await userManager.ReplaceClaimAsync(user, claimName, new Claim("Name", employeeRequest.Name));

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        result = await userManager.ReplaceClaimAsync(user, claimCode, new Claim("EmployeeCode", employeeRequest.EmployeeCode));

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Ok();
    }
}
