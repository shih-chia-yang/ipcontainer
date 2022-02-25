namespace practice.domain.Kernel.Services;

public interface IEmailService
{
    Task<bool> Send();
}