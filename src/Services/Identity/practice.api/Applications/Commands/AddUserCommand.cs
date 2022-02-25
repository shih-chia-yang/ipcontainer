namespace practice.api.Applications.Commands;

public class AddUserCommand:IEventRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
}

public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<AddUserCommand> _logger;

    public AddUserCommandHandler(
        IUserRepository repo,
        IMapper mapper,
        ILogger<AddUserCommand> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<IResponse> Handle(AddUserCommand request)
    {
        var userExist = _repo.Get(request.Email).IsEmpty is false;
        if(userExist)
            return new CommandResponse(false, userExist,new string[]{"this email already exist"});
        var newUser = User.CreateNew(request.FirstName, request.LastName, request.Email,request.Password);
        _logger.LogInformation($"{request.FirstName} {request.LastName} registered");
        _repo.Add(newUser);
        var exec=await _repo.UnitOfWork.SaveChangesAsync();
         var result = this._mapper.Map<UserProfileViewModel>(newUser);
        return new CommandResponse(exec>0?true:false,result,Enumerable.Empty<string>());
    }
}