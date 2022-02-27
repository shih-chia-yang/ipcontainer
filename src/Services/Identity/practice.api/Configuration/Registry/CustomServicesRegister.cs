using Microsoft.Extensions.Options;
using practice.api.Applications.Services;
using practice.api.Configuration.Swagger;
using practice.domain.Kernel;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace practice.api.Configuration.Registry;

public static partial class StartUpRegister
{
    public static WebApplicationBuilder RegisterCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddTransient<IRequestHandler<AddUserCommand>, AddUserCommandHandler>();
        builder.Services.AddTransient<IRequestHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
        builder.Services.AddTransient<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddTransient<IAuthenticateManager, AuthenticateManager>();
        builder.Services.AddSingleton<IDispatcher, EventDispatcher>();
        builder.Services.AddSingleton<ISubscriptionManager, SubscriptionManager>();
        builder.Services.AddSingleton<IEventBus, EventBus>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return builder;
    }
}