using Autofac;
using Collectively.Common.ServiceClients;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Framework.IoC
{
    public class ServiceClientsModule : ServiceClientsModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterService<RemarkServiceClient, IRemarkServiceClient>(builder, "remarks");
        }
    }
}