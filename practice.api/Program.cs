using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using practice.api.Configuration.Models;

var builder = WebApplication.CreateBuilder(args);
var configureOptions = builder.Configuration.GetSection(nameof(JwtConfig));
builder.Services.Configure<JwtConfig>(configureOptions);
var jwtConfig = new JwtConfig();
configureOptions.Bind(jwtConfig);
// Add services to the container.
builder.Services.AddDbContext<UserContext>(options=>options.UseInMemoryDatabase("identity_app"));
builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
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
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEventHandler<AddUserCommand, User>, AddUserCommandHandler>();
builder.Services.AddTransient<IEventHandler<UpdateUserCommand, User>, UpdateUserCommandHandler>();
builder.Services.AddTransient<IEventHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>();

var app = builder.Build();
var scope=app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<UserContext>();
await DbInitials.SeedAsync(context);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
