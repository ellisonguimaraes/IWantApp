using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IWantApp.Services;

public class JwTUtils : IJwTUtils
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwTUtils(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateAccessToken(IdentityUser user)
    {
        // Subject create with userClaims
        var userClaims = await _userManager.GetClaimsAsync(user);

        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        });

        subject.AddClaims(userClaims);

        // Secret key
        var key = Encoding.ASCII.GetBytes(_configuration["JwtBearerTokenSettings:SecretKey"]);

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["JwtBearerTokenSettings:Audience"],
            Issuer = _configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddSeconds(double.Parse(_configuration["JwtBearerTokenSettings:ExpiryTimeInSeconds"]))
        };

        // Generating token
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
