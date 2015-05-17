using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient.Base
{
    public abstract class BaseHubClient
    {
        protected readonly IHubProxy _userHub;

        public BaseHubClient(HubConnection hubConnection, string hubName)
        {
            _userHub = hubConnection.CreateHubProxy(hubName);
        }

    }
}