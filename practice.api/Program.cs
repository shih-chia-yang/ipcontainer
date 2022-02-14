using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserContext>(options=>options.UseInMemoryDatabase("identity_app"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEventHandler<AddUserCommand, User>, AddUserCommandHandler>();
builder.Services.AddTransient<IEventHandler<UpdateUserCommand, User>, UpdateUserCommandHandler>();
builder.Services.AddTransient<IEventHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var scope=app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<UserContext>();
    await DbInitials.SeedAsync(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
