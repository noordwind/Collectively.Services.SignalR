using System.Linq;
using System.Threading.Tasks;
using Collectively.Messages.Events.Operations;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.SignalR.Hubs;
using Collectively.Services.SignalR.Models;
using Collectively.Services.SignalR.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Collectively.Services.SignalR.Services
{
    public class HubService : IHubService
    {
        private readonly IHubContext<CollectivelyHub> _hubContext;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public HubService(IHubContext<CollectivelyHub> hubContext,
            IRemarkServiceClient remarkServiceClient)
        {
            _hubContext = hubContext;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task PublishOperationUpdatedAsync(OperationUpdated @event)
            => await _hubContext.Clients.All.InvokeAsync("operation_updated",
                new
                {
                    requestId = @event.RequestId,
                    name = @event.Name,
                    state = @event.State,
                    code = @event.Code,
                    message = @event.Message
                }
            );

        public async Task PublishPhotosToRemarkAddedAsync(PhotosToRemarkAdded @event)
        {
            var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
            if (remarkDto.HasNoValue)
            {
                return;
            }
            var remark = remarkDto.Value;
            await _hubContext.Clients.All.InvokeAsync("photos_to_remark_added",
                new
                {
                    remarkId = remark.Id,
                    newPhotos = remark.Photos
                        .Skip(remark.Photos.Count - 3)
                        .Take(3)
                        .Select(x => new 
                        {
                            size = x.Size,
                            groupId = x.GroupId,
                            url = x.Url
                        })
                }
            );
        }

        public async Task PublishPhotosFromRemarkRemovedAsync(PhotosFromRemarkRemoved @event)
        {
            var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
            if (remarkDto.HasNoValue)
            {
                return;
            }
            var remark = remarkDto.Value;
            await _hubContext.Clients.All.InvokeAsync("photos_from_remark_removed",
                new
                {
                    remarkId = remark.Id,
                    groupdIds = remark.Photos.Select(x => x.GroupId)
                }
            );
        }

        public async Task PublishRemarkVoteSubmittedAsync(RemarkVoteSubmitted @event)
            => await _hubContext.Clients.All.InvokeAsync("remark_vote_submitted",
                new
                {
                    remarkId = @event.RemarkId,
                    userId = @event.UserId,
                    positive = @event.Positive,
                    createdAt = @event.CreatedAt
                }
            );

        public async Task PublishRemarkVoteDeletedAsync(RemarkVoteDeleted @event)
            => await _hubContext.Clients.All.InvokeAsync("remark_vote_deleted",
                new
                {
                    remarkId = @event.RemarkId,
                    userId = @event.UserId,
                }
            );
    }
}