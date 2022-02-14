
namespace practice.api.Controllers.Applications.Commands;

public class UpdateUserCommand:IEventRequest
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Organization { get; set; }
    public string Unit { get; set; }
}

public class UpdateUserCommandHandler : IEventHandler<UpdateUserCommand, User>
{
    private readonly IUserRepository _repo;

    public UpdateUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public User Handle(UpdateUserCommand request)
    {
        var user = _repo.Get(request.Email);
        if(string.IsNullOrEmpty(user.Email))
            return user;
        user.UpdateEmail(request.Email);
        user.UpdatePhone(request.Phone);
        user.UpdateOrganization(request.Organization);
        user.UpdateUnit(request.Unit);
        _repo.Update(user);
        return user;
    }
}
