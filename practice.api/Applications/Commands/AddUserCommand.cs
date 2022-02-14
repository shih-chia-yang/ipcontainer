namespace practice.api.Controllers.Applications.Commands;

public class AddUserCommand:IEventRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class AddUserCommandHandler : IEventHandler<AddUserCommand,User>
{
    private readonly IUserRepository _repo;

    public AddUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public User Handle(AddUserCommand request)
    {
        var user = User.CreateNew(request.FirstName, request.LastName, request.Email);
        _repo.Add(user);
        return user;
    }
}