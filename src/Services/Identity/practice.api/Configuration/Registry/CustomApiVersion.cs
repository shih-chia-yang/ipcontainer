using Microsoft.AspNetCore.Mvc.Versioning;

namespace practice.api.Configuration.Registry;

public static partial class StartUpRegister
{
    public static WebApplicationBuilder SetCustomApiVersion(this WebApplicationBuilder builder)
    {
        builder.Services.AddVersionedApiExplorer(options=>
        {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
        });
        builder.Services.AddApiVersioning(options=>{
            //With the AssumeDefaultVersionWhenUnspecified and DefaultApiVersion properties,
            //we are accepting version 1.0 if a client doesn’t specify the version of the API.
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = ApiVersion.Default;
            //Additionally, by populating the ReportApiVersions property, 
            //we show actively supported API versions. 
            //It will add both api-supported-versions and api-deprecated-versions headers to our response.
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                //https://localhost:7088/api/User/users?api-version=1.0
                new QueryStringApiVersionReader("api-version"),
                //in headers set X-Version property
                //X-Version:1.0
                new HeaderApiVersionReader("X-Version"),
                //find accept in the headers, then add ver=1.0
                new MediaTypeApiVersionReader("ver"));
        });
        return builder;
    }
}