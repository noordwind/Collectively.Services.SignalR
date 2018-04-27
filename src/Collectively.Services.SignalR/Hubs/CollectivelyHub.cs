using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR.Hubs
{
    public class CollectivelyHub : Hub
    {
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

                    return;
                }
                jwt = maybeJwt.Value;
            }
            catch
            {
                await DisconnectAsync();

                return;
            }
            await Groups.AddAsync(Context.ConnectionId, jwt.Subject);
        }

        private async Task DisconnectAsync()
        {
            await this.Clients.Client(this.Context.ConnectionId).SendAsync("disconnect");
        }
    }
}