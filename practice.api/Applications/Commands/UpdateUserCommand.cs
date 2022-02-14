
namespace practice.api.Applications.Commands;

public class UpdateUserCommand:IEventRequest
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
}

public class UpdateUserCommandHandler : IEventHandler<UpdateUserCommand, User>
{
    private readonly IUserRepository _repo;

    public UpdateUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public async Task<User> HandleAsync(UpdateUserCommand request)
    {
        var user = _repo.Get(request.Email);
        if(user.IsEmpty is true)
            return user;
        user.UpdateEmail(request.Email);
        user.UpdatePhone(request.Phone);
        user.UpdateOrganization(request.Organization);
        user.UpdateUnit(request.Unit);
        _repo.Update(user);
        await _repo.UnitOfWork.SaveChangesAsync();
        return user;
    }
}
