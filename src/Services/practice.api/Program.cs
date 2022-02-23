using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using practice.api.Configuration.Models;
using practice.domain.Kernel;
using practice.domain.Kernel.Command;

var builder = WebApplication.CreateBuilder(args);
var configureOptions = builder.Configuration.GetSection(nameof(JwtConfig));
builder.Services.Configure<JwtConfig>(configureOptions);
var jwtConfig = new JwtConfig();
configureOptions.Bind(jwtConfig);
// Add services to the container.
builder.Services.AddDbContext<UserContext>(options=>options.UseInMemoryDatabase("identity_app"));

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddTransient<IRequestHandler<AddUserCommand>, AddUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteUserCommand>, DeleteUserCommandHandler>();
builder.Services.AddSingleton<IDispatcher, EventDispatcher>();
builder.Services.AddSingleton<ISubscriptionManager, SubscriptionManager>();
builder.Services.AddSingleton<IEventBus, EventBus>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var scope=app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<UserContext>();
    if(context != null)
        await DbInitials.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
