using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SampleJTWAuthNoMiddleware.Configure
{
    public static class ConfigureJWTAuthentication
    {
        public static void ConfigureJWTAuthenticationService(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => 
                {
                    //TODO: replace with much better key and also where it is stored
                    var key = Encoding.ASCII.GetBytes("ThisIsMyCustomSecretKeyAuthenticationSampleKey");
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero, //It forces token to expire exactly at the token expiration time instead of waiting 5 minutes
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }
    }
}