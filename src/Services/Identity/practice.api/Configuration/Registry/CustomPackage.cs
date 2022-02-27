namespace practice.api.Configuration.Registry;

public static partial class StartUpRegister
{
    public static WebApplicationBuilder AddPackage(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return builder;
    }
}
