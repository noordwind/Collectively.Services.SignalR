using System;
using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Services.SignalR.Hubs;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Handlers
{
    public abstract class HandlerBase<T> : IEventHandler<T> where T : IEvent
    {
        private readonly IHandler _handler;
        private readonly IHubService _hubService;
        private readonly Func<T, Task> _handleAsync;

        public HandlerBase(IHandler handler,
            IHubService hubService, Func<T, Task> handleAsync)
        {
            _handler = handler;
            _hubService = hubService;
            _handleAsync = handleAsync;
        }
        
        public async Task HandleAsync(T @event)
            => await _handler
                .Run(async () => await _handleAsync(@event))
                .OnError((ex, logger) => 
                    logger.Error(ex, $"Error occured while publishing {@event.GetType().Name} event."))
                .ExecuteAsync();
    }
}