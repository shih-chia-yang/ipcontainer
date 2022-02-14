namespace practice.api.Applications.Commands;

public class DeleteUserCommand:IEventRequest
{
    public string Email { get; set; }
}

public class DeleteUserCommandHandler : IEventHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _repo;

    public DeleteUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public async Task<bool> HandleAsync(DeleteUserCommand request)
    {
        var user=_repo.Get(request.Email);
        if(user.IsEmpty is true)
            return false;
        _repo.Delete(user.Email);
        var result =await _repo.UnitOfWork.SaveChangesAsync();
        return true;
    }
}
