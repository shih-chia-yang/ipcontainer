namespace practice.api.Applications.Commands;

public class DeleteUserCommand:IEventRequest
{
    public string Email { get; set; } = string.Empty;
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(
        IUserRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<IResponse> Handle(DeleteUserCommand request)
    {
        var user=_repo.Get(request.Email);
        if(user.IsEmpty is true)
            return new CommandResponse(false,user,Enumerable.Empty<string>());
        _repo.Delete(user.Email);
        var result =await _repo.UnitOfWork.SaveChangesAsync();
        var model = _mapper.Map<UserProfileViewModel>(user);
        return new CommandResponse(result>0?true:false,model,Enumerable.Empty<string>());
    }
}
