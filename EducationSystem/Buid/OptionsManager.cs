using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EducationSystem.Web.Api
{
    internal class OptionsManager
    {
        private WebApplicationBuilder builder;

        public OptionsManager(WebApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public void JWTOption(JwtBearerOptions configure)
        {
            var section = builder.Configuration.GetSection("JWTTokenValidate");
            configure.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = section.GetSection("Issuer").Value,
                ValidAudience = section.GetSection("Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section.GetSection("Key").Value))
                
            };
        }

        public void CorsOption(CorsOptions configure)
        {
            var section = builder.Configuration.GetSection("Cors");
            configure.AddPolicy(section.GetSection("Name").Value, builder => builder
                     .WithOrigins(section.GetSection("WithOrigins").Value)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
        }

        public void AuthenticationOption(AuthenticationOptions configure)
        {
            configure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            configure.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            configure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public void DbContextOption(DbContextOptionsBuilder configure)
        {
            var sectionConectionString = builder.Configuration.GetSection("ConectionDataBase");
            configure.UseSqlServer(sectionConectionString.GetSection("ConectionString").Value);
        }

        public void IdentityUserOption(IdentityOptions configure)
        {
            var sectionValidatePassword = builder.Configuration.GetSection("ValidatePassword");

            configure.Password.RequiredLength = int.Parse(sectionValidatePassword.GetSection("RequiredLength").Value);
            configure.Password.RequireNonAlphanumeric = bool.Parse(sectionValidatePassword.GetSection("isRequireNonAlphanumeric").Value);
            configure.Password.RequireLowercase = bool.Parse(sectionValidatePassword.GetSection("RequireLowercase").Value);
            configure.Password.RequireUppercase = bool.Parse(sectionValidatePassword.GetSection("RequireUppercase").Value);
            configure.Password.RequireDigit = bool.Parse(sectionValidatePassword.GetSection("isRequireDigit").Value);

            configure.User.RequireUniqueEmail = true; // defaults
        }
    }
}
