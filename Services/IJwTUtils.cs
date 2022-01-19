using Microsoft.AspNetCore.Identity;

namespace IWantApp.Services;

public interface IJwTUtils
{
    string GenerateAccessToken(IdentityUser user);
}
