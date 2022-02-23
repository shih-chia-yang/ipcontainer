using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using practice.domain.Kernel.Command;
using practice.domain.Kernel.Events;

namespace practice.domain.unittests.Kernel;

public class TestEventRequest:IEventRequest
{
    public string Message { get; set; }
}

public class TestEventHandler : IRequestHandler<TestEventRequest>
{

    // public Task<IResponse> Handle(TestEventRequest request)
    // {
    //    
    // }
    public async Task<IResponse> Handle(TestEventRequest request)
    {
         return  new CommandResponse(true, $"{request.Message} received", null);
    }
}
