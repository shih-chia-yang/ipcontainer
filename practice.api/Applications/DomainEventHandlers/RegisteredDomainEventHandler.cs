using practice.domain.Events;

namespace practice.api.Applications.DomainEventHandlers;

public class RegisteredDomainEventHandler : IIntegrationEventHandler<RegisteredDomainEvent>
{
    private readonly UserRepository _repo;

    public RegisteredDomainEventHandler(UserRepository repo)
    {
        _repo=repo;
    }

    public async Task Handle(RegisteredDomainEvent request)
    {
        var userExist = _repo.Get(request.Email);
        if(userExist.IsEmpty is true)
        {
            // return this email already in use"
        }
        var newUser = User.CreateNew(request.FirstName, request.LastName, request.Email);
        _repo.Add(newUser);
        var count = await _repo.UnitOfWork.SaveChangesAsync();
    }
}
