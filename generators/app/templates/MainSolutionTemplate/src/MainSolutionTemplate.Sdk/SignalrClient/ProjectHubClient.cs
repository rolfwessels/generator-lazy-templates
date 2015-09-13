using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.SignalrClient.Base;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using MainSolutionTemplate.Utilities.Helpers;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace MainSolutionTemplate.Sdk.SignalrClient
{

    public class NotificationHubClient : BaseHubClient
    {

        public NotificationHubClient(HubConnection hubConnection) : base(hubConnection, "NotificationHub")
        {
            
        }

        public async Task Subscribe<T>(string name, Action<ValueUpdateModel<T>> callback)
        {
            _hub.On("OnUpdate", callback);
            await _hub.Invoke("SubscribeToUpdates", name);
        }


    }
}