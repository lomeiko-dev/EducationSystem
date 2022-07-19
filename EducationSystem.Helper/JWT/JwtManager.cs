using EducationSystem.Helper.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationSystem.Helper.JWT
{
    public class JwtManager
    {
        private readonly OptionsJwtValidate optionsJwtValidate;

        public JwtManager(IOptions<OptionsJwtValidate> optionsJwtValidate)
        {
            this.optionsJwtValidate = optionsJwtValidate.Value;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            var validationParametrs = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsJwtValidate.Key)),
                ValidIssuer = optionsJwtValidate.Issuer,
                ValidAudience = optionsJwtValidate.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true
            };

            var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, validationParametrs, out SecurityToken validatedToken);
                return principal;
            
        }

        public string GenerateToken(List<Claim> claims, GeneratorType type)
        {
            var jwt = new JwtSecurityToken(
            issuer: optionsJwtValidate.Issuer,
            audience: optionsJwtValidate.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(type == 0 ? 
                                                                              int.Parse(optionsJwtValidate.ExpiresAccess) : 
                                                                              int.Parse(optionsJwtValidate.ExpiresRefresh))),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsJwtValidate.Key)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
