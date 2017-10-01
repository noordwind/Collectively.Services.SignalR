using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Collectively.Services.SignalR.Hubs
{
    public class CollectivelyHub : Hub
    {
        private readonly ConcurrentDictionary<string, ISet<string>> _users = 
            new ConcurrentDictionary<string, ISet<string>>();
        private readonly IJwtTokenHandler _jwtHandler;

        public CollectivelyHub(IJwtTokenHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public async Task InitializeAsync(string token)
        {
            if (token.Empty())
            {
                await DisconnectAsync();
            }
            JwtDetails jwt = null;
            try
            {
                var maybeJwt = _jwtHandler.Parse(token);
                if (maybeJwt.HasNoValue)
                {
                    await DisconnectAsync();
                }
                jwt = maybeJwt.Value;
            }
            catch
            {
                await DisconnectAsync();
            }
            var userId = jwt.Subject;
            var connections = new HashSet<string>();
            if(!_users.ContainsKey(userId))
            {
                _users[userId] = connections;
            }
            _users[userId].Add(Context.ConnectionId);
        }

        private async Task DisconnectAsync()
        {
            await this.Clients.Client(this.Context.ConnectionId).InvokeAsync("disconnect");
        }
    }
}