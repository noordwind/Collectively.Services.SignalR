using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Handlers
{
    public class PhotosFromRemarkRemovedHandler : HandlerBase<PhotosFromRemarkRemoved>
    {
        public PhotosFromRemarkRemovedHandler(IHandler handler,
            IHubService hubService) : base(handler, hubService,
                x => hubService.PublishPhotosFromRemarkRemovedAsync(x))
        {
        }
    }
}