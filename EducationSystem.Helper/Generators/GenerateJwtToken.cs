using EducationSystem.Helper.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationSystem.Helper.Generators
{
    public class GenerateJwtToken
    {
        private readonly OptionsJwtValidate optionsJwtValidate;

        public GenerateJwtToken(IOptions<OptionsJwtValidate> optionsJwtValidate)
        {
            this.optionsJwtValidate = optionsJwtValidate.Value;
        }

        public bool IsValid(string refreshToken)
        {
            var validationParametrs = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsJwtValidate.Key)),
                ValidIssuer = optionsJwtValidate.Issuer,
                ValidAudience = optionsJwtValidate.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            var handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(refreshToken, validationParametrs, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            { return false; }
        }

        public string CreateToken(int minute, List<Claim> claims = null)
        {
            var jwt = new JwtSecurityToken(
            issuer: optionsJwtValidate.Issuer,
            audience: optionsJwtValidate.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(minute)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsJwtValidate.Key)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
