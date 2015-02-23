using System;
using MainSolutionTemplate.Dal.Models.Enums;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IConnectionStateMapping
	{
		int Count { get; }
		ConnectionState AddOrGet(HubCallerContext context);
		ConnectionState Remove(string connectionId);
		void Reconnect(HubCallerContext context);
		bool IsAuthorized(ConnectionState connectionState, Activity[] activities);
	}
}