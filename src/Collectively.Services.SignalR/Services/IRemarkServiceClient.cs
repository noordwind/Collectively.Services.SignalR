using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Services.SignalR.Services
{
    public interface IRemarkServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(Guid id) where T : class;
    }
}