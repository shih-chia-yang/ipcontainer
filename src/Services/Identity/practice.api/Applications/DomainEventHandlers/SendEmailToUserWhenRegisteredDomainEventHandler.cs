using practice.domain.Kernel.Services;

namespace practice.api.Applications.DomainEventHandlers;

public class SendEmailToUserWhenRegisteredDomainEventHandler 
    : INotificationHandler<RegisteredDomainEvent>
{
    private readonly IEmailService _email;
    private readonly ILogger<SendEmailToUserWhenRegisteredDomainEventHandler> _logger;

    public SendEmailToUserWhenRegisteredDomainEventHandler(
        IEmailService email,
        ILogger<SendEmailToUserWhenRegisteredDomainEventHandler> logger)
    {
        _email = email;
        _logger = logger;
    }

    public async Task Handle(RegisteredDomainEvent request)
    {
        await _email.Send();
        _logger.LogTrace($"{request.FirstName} {request.LastName} have registered ,and send notification mail to {request.Email}");

    }
}
