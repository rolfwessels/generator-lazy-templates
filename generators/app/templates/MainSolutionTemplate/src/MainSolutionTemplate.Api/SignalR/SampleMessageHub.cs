using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Api.SignalR
{
    public class SampleMessageHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}