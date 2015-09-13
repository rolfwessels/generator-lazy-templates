using System;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Enums;
using MainSolutionTemplate.Utilities.Helpers;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Hubs
{
    
    public class NotificationHub : BaseHub
    {
        private readonly IMessenger _messenger;

        public NotificationHub(IConnectionStateMapping connectionStateMapping , IMessenger messenger)
            : base(connectionStateMapping)
        {
            _messenger = messenger;
        }

        

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public Task SubscribeToUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var connection = _connectionsState.AddOrGet(Context);
                var makeGenericType = GetTypeFromName(typeName);
                _messenger.Register(makeGenericType, connection, CallBackToClient);
            });
        }

        
       
        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public Task UnsubscribeFromUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var makeGenericType = GetTypeFromName(typeName);
                var connection = _connectionsState.AddOrGet(Context);
                _messenger.Unregister(makeGenericType, connection);
            });
        }

        #endregion

        private static Type GetTypeFromName(string typeName)
        {
            var type = MyReflectionHelper.FindOfType(typeof(Project).Assembly, typeName);
            var makeGenericType = MyReflectionHelper.MakeGenericType(typeof(DalUpdateMessage<>), type);
            return makeGenericType;
        }

        private void CallBackToClient(object obj)
        {
            Clients.Caller.OnUpdate(obj);
        }

        
    }
}