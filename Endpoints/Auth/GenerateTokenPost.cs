using IWantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Auth;

public class GenerateTokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethods.Post.ToString() };
    public static Delegate Handle => Action;
    
    [AllowAnonymous]
    public static async Task<IResult> Action([FromBody] LoginRequest loginRequest, 
                                 [FromServices] UserManager<IdentityUser> userManager,
                                 [FromServices] IJwTUtils jwTUtils)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);

        if (user == null)
            return Results.BadRequest();

        if (!(await userManager.CheckPasswordAsync(user, loginRequest.Password)))
            return Results.BadRequest();

        var token = await jwTUtils.GenerateAccessTokenAsync(user);

        return Results.Ok(token);
    }
}
