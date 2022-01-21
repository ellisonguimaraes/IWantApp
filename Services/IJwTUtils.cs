using Microsoft.AspNetCore.Identity;

namespace IWantApp.Services;

public interface IJwTUtils
{
    Task<string> GenerateAccessTokenAsync(IdentityUser user);
}
