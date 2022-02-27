using Microsoft.Extensions.Options;
using practice.api.Configuration.Models;

namespace practice.api.Configuration.Registry;

public static partial class StartUpRegister
{
    public static WebApplicationBuilder SetConfigOptions(this WebApplicationBuilder builder)
    {
        var configureOptions = builder.Configuration.GetSection(nameof(JwtConfig));
        builder.Services.Configure<JwtConfig>(configureOptions);
        var jwtConfig = new JwtConfig();
        configureOptions.Bind(jwtConfig);
        
        return builder;
    }
}