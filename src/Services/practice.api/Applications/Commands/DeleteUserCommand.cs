namespace practice.api.Applications.Commands;

public class DeleteUserCommand:IEventRequest
{
    public string Email { get; set; } = string.Empty;
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo;

    public DeleteUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }
    public async Task<IResponse> Handle(DeleteUserCommand request)
    {
        var user=_repo.Get(request.Email);
        if(user.IsEmpty is true)
            return new CommandResponse(false,user,null);
        _repo.Delete(user.Email);
        var result =await _repo.UnitOfWork.SaveChangesAsync();
        return new CommandResponse(true,user,null);
    }
}
