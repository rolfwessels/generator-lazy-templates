using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.SignalrClient.Base;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

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

        public async Task Unsubscribe(string name)
        {
            await _hub.Invoke("UnsubscribeFromUpdates", name);
        }
    }
}