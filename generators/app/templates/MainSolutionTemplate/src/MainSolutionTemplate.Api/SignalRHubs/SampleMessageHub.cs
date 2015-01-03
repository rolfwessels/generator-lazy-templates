using Microsoft.AspNet.SignalR;

namespace MainSolutionTemplate.Web.SignalRHubs
{
    public class SampleMessageHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}