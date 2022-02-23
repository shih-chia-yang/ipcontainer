using practice.domain.Events;

namespace practice.api.Applications.DomainEventHandlers;

public class RegisteredDomainEventHandler : IIntegrationEventHandler<RegisteredDomainEvent>
{
    private readonly UserRepository _repo;
    private readonly IEventBus _eventBus;

    public RegisteredDomainEventHandler(UserRepository repo,
    IEventBus eventBus)
    {
        _repo=repo;
        _eventBus = eventBus;
    }

    public async Task Handle(RegisteredDomainEvent request)
    {
        var userExist = _repo.Get(request.Email);
        if(userExist.IsEmpty is true)
        {
            // return this email already in use"
        }
        var result = await _eventBus.Send(new AddUserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        });
    }
}
