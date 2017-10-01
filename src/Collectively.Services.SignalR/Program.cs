using Collectively.Common.Host;
using Collectively.Messages.Events.Operations;
using Collectively.Messages.Events.Remarks;

namespace Collectively.Services.SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(args: args)
                .UseAutofac(Startup.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<OperationUpdated>()
                .SubscribeToEvent<PhotosToRemarkAdded>()
                .SubscribeToEvent<PhotosFromRemarkRemoved>()
                .SubscribeToEvent<RemarkVoteSubmitted>()
                .SubscribeToEvent<RemarkVoteDeleted>()
                // .SubscribeToEvent<RemarkCreated>()
                // .SubscribeToEvent<RemarkResolved>()
                // .SubscribeToEvent<RemarkDeleted>()
                // .SubscribeToEvent<RemarkProcessed>()
                // .SubscribeToEvent<RemarkRenewed>()
                .Build()
                .Run();
        }
    }
}
