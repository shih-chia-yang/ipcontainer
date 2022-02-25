using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace practice.api.Configuration.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
    this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
            description.GroupName,
                new OpenApiInfo()
                {
                    Title = $"Practice API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated ? "This Api version has been deprecated" : string.Empty,
                    TermsOfService= new Uri("https://ulist.moe.gov.tw"),
                    Contact = new OpenApiContact{
                        Name="Chia-yang,Shih",
                        Email="heipuser@yuntech.edu.tw",
                        Url=new Uri("https://ulist.moe.gov.tw/Home/Contact")
                    },
                    License =new OpenApiLicense{
                        Name="Use under M.O.E",
                        Url=new Uri("https://depart.moe.edu.tw/ed2200/")
                    }
                }
                );
                var xmlFile=$"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath=Path.Combine(AppContext.BaseDirectory,xmlFile);
                options.IncludeXmlComments(xmlPath);
        }
    }
}