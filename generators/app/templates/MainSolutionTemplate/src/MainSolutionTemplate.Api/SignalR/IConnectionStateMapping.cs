using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IConnectionStateMapping
	{
		int Count { get; }
		void Add(HubCallerContext context);
		ConnectionState Add(HubCallerContext context , Action<ConnectionState> connectionBuild);
		ConnectionState Remove(string connectionId);
		void Reconnect(HubCallerContext context);
	}
}