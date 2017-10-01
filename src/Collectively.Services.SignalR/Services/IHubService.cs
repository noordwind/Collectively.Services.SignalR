using System.Threading.Tasks;
using Collectively.Messages.Events.Operations;
using Collectively.Messages.Events.Remarks;

namespace Collectively.Services.SignalR.Services
{
    public interface IHubService
    {
        Task PublishOperationUpdatedAsync(OperationUpdated @event);
        Task PublishPhotosToRemarkAddedAsync(PhotosToRemarkAdded @event);
        Task PublishPhotosFromRemarkRemovedAsync(PhotosFromRemarkRemoved @event);
        Task PublishRemarkVoteSubmittedAsync(RemarkVoteSubmitted @event);
        Task PublishRemarkVoteDeletedAsync(RemarkVoteDeleted @event);
    }
}