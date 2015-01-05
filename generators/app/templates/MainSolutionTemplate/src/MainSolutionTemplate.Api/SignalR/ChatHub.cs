using System.Reflection;
using Microsoft.AspNet.SignalR;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	public class ChatHub : Hub
    {
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public void Send(string data, string message)
        {
			_log.Info(string.Format("data:'{0}' '{1}'", data, message));
			Clients.All.addMessage(data, message);


        }
    }
}