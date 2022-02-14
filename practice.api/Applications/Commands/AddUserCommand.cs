using practice.domain.Kernel.Repository;

namespace practice.api.Applications.Commands;

public class AddUserCommand:IEventRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AddUserCommandHandler : IEventHandler<AddUserCommand,User>
{
    private readonly IUserRepository _repo;

    public AddUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public async Task<User> HandleAsync(AddUserCommand request)
    {
        var user = User.CreateNew(request.FirstName, request.LastName, request.Email);
        _repo.Add(user);
        await _repo.UnitOfWork.SaveChangesAsync();
        return user;
    }
}