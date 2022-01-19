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
    public static IResult Action([FromBody] LoginRequest loginRequest, 
                                 [FromServices] UserManager<IdentityUser> userManager,
                                 [FromServices] IJwTUtils jwTUtils)
    {
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;

        if (user == null)
            return Results.BadRequest();

        if (!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            return Results.BadRequest();

        return Results.Ok(jwTUtils.GenerateAccessToken(user));
    }
}
