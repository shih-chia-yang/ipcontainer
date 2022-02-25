namespace practice.domain.Kernel.Services;

public class EmailService : IEmailService
{
    public Task<bool> Send()
    {
        return Task.FromResult(true);
    }
}