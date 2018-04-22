using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SecureAspnetCoreApi.JWTAuthentication.Configuration
{
    public static class JwtBearerConfiguration
    {
        public static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, string issuer, string audience, string secret)
        {
            builder.AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero //validate time immediately in Expire field because of it's default value is 5 minutes
                };
            });

            return builder;
        }
    }
}
