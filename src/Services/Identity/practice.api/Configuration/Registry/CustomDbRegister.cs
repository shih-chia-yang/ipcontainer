namespace practice.api.Configuration.Registry;

public static partial class StartUpRegister
{
    public static WebApplicationBuilder AddCustomDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserContext>(options=>options.UseInMemoryDatabase("identity_app"));
        return builder;
    }
}