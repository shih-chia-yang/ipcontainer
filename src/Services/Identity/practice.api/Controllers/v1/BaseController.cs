using practice.api.Applications.Results;
using practice.domain.Kernel.Repository;

namespace practice.api.v1.Controllers;

[Route("api/v{}version:apiVersion")]
[ApiController]
[ApiVersion("1.0")]
public class BaseController:ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public BaseController(
        IUnitOfWork unitOfWork,
        IEventBus eventBus
    )
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    internal ResponseError Error(int code,string type,string message)
    {
        return ResponseError.CreateNew(code, message, type);
    }
}