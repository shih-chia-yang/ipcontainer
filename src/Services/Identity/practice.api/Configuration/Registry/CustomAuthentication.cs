using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using practice.api.Configuration.Models;

namespace practice.api.Configuration.Registry;


public static partial class StartUpRegister
{
    public static WebApplicationBuilder SetJwtAuthentication(
        this WebApplicationBuilder builder)
    {
        var provider =builder.Services.BuildServiceProvider().CreateScope().ServiceProvider;
        var config=provider.GetService<IOptions<JwtConfig>>();
        var jwtConfig = config.Value;
        var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);
        var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, //todo update
                // ValidIssuer = "",
                ValidateAudience = false, //todo update
                // ValidAudience = "",
                RequireExpirationTime=false, //todo update
                ValidateLifetime=true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };
        builder.Services.AddSingleton(tokenValidationParameters);

        builder.Services.AddAuthentication(options=>{
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options=>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
        });
        return builder;
    }
}