using Microsoft.EntityFrameworkCore;
using practice.domain.AggregateModel.Entities;

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
                    User.CreateNew("andy","wang","andy@test.com","andy"),
                    User.CreateNew("bod","chen","bob@test.com","bod"),
                    User.CreateNew("carol","chang","carol@test.com","carol")
                }
            );
        }

        await context.SaveChangesAsync();
    }
}