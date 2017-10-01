using System;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients;
using Collectively.Common.Types;

namespace Collectively.Services.SignalR.Services
{
    public class RemarkServiceClient : IRemarkServiceClient
    {
        private readonly IServiceClient _serviceClient;
        private readonly string _name;

        public RemarkServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<T>> GetAsync<T>(Guid id) where T : class 
            => await _serviceClient.GetAsync<T>(_name, $"remarks/{id}");
    }
}