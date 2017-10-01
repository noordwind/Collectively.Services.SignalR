using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Handlers
{
    public class PhotosToRemarkAddedHandler : HandlerBase<PhotosToRemarkAdded>
    {
        public PhotosToRemarkAddedHandler(IHandler handler,
            IHubService hubService) : base(handler, hubService,
                x => hubService.PublishPhotosToRemarkAddedAsync(x))
        {
        }
    }
}