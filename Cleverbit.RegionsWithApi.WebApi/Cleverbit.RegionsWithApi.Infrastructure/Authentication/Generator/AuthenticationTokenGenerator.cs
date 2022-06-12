using Cleverbit.RegionsWithApi.Common.Utility.Consts;
using Cleverbit.RegionsWithApi.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cleverbit.RegionsWithApi.Infrastructure.Authentication.Generator
{
    public class AuthenticationTokenGenerator
    {
        internal static T GenerateAuthenticationToken<T>(T response)
           where T : AuthenticationResponse
        {
            var claims = new[]
            {
                new Claim(AppClaimTypes.UserId, response.UserId.ToString()),
                new Claim(AppClaimTypes.Email, response.Email.ToString())
            }
            .ToList();

            var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(30),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RegionsWithApi#12.06.2022")), SecurityAlgorithms.HmacSha256)
                );

            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.ExpiryDate = token.ValidTo;

            return response;
        }
    }
}
