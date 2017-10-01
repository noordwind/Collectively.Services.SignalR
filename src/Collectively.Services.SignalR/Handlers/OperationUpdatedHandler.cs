using Collectively.Common.Services;
using Collectively.Messages.Events.Operations;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Handlers
{
    public class OperationUpdatedHandler : HandlerBase<OperationUpdated>
    {
        public OperationUpdatedHandler(IHandler handler,
            IHubService hubService) : base(handler, hubService,
                x => hubService.PublishOperationUpdatedAsync(x))
        {
        }
    }
}