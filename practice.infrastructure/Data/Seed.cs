using Microsoft.EntityFrameworkCore;
using practice.domain.Entities;

namespace practice.infrastructure.Data;

public static class DbInitials
{
    public static async Task SeedAsync(UserContext context)
    {
        if(context.Users.Any()==false)
        {
            await context.AddRangeAsync(
                new List<User>()
                {
                    User.CreateNew("andy","wang","andy@test.com"),
                    User.CreateNew("bod","chen","bob@test.com"),
                    User.CreateNew("carol","chang","carol@test.com")
                }
            );
        }

        await context.SaveChangesAsync();
    }
}