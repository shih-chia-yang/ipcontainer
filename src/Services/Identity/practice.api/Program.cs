using System.Configuration;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using practice.api.Configuration.Registry;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder
.SetConfigOptions()
.AddCustomDb()
.RegisterCustomServices()
.SetApiVersion()
.SetJwtAuthentication()
.AddPackage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    var scope = app.Services.CreateScope();
    var provider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options => {
                // options.SwaggerEndpoint("/swagger/v1/swagger.json", "api_version v1");
                foreach(var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",description.GroupName.ToUpperInvariant());
                }
            });
    
    var context = scope.ServiceProvider.GetService<UserContext>();
    if(context != null)
        await DbInitials.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
