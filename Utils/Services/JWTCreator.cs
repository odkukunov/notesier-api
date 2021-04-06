using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notesier_API.Utils.Services
{
    public static class JWTCreatorExtension
    {
        public static void AddJWTHandler(this IServiceCollection services)
        {
            services.AddSingleton<JWTHandler>();
        }
    }

    public class JWTHandler
    {

        public string Generate(IEnumerable<Claim> claims, DateTime expires)
        {
            var jwt = new JwtSecurityToken(issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: expires,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal Validate(string token)
        {
            SecurityToken securityToken;
            ClaimsPrincipal claims = new JwtSecurityTokenHandler()
                                        .ValidateToken(token, new TokenValidationParameters()
                                        {
                                            ValidateIssuer = true,
                                            ValidIssuer = AuthOptions.ISSUER,
                                            ValidateAudience = true,
                                            ValidAudience = AuthOptions.AUDIENCE,
                                            ValidateLifetime = true,
                                            IssuerSigningKey = AuthOptions.GetSecurityKey(),
                                            ValidateIssuerSigningKey = true
                                        }, out securityToken);

            return claims;
        }
    }
}
