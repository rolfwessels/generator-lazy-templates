using System;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using log4net;
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
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMessenger _messenger;

        public NotificationHub(IConnectionStateMapping connectionStateMapping , IMessenger messenger)
            : base(connectionStateMapping)
        {
            _messenger = messenger;
        }

        

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.Subscribe)]
        public Task SubscribeToUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var connection = _connectionsState.AddOrGet(Context);
                var makeGenericType = GetTypeFromName(typeName);
                _log.Debug("NotificationHub:SubscribeToUpdates " + typeName);
                _messenger.Register(makeGenericType, connection, (obj) => CallBackToClient(typeName,obj));
            });
        }


        [HubAuthorizeActivity(Activity.Subscribe)]
        public Task UnsubscribeFromUpdates(string typeName)
        {
            return Task.Run(() =>
            {
                var makeGenericType = GetTypeFromName(typeName);
                var connection = _connectionsState.AddOrGet(Context);
                _log.Debug("NotificationHub:UnsubscribeFromUpdates " + typeName);
                _messenger.Unregister(makeGenericType, connection);
            });
        }

        #region Overrides of BaseHub

        public override Task OnDisconnected(bool stopCalled)
        {
            var connection = _connectionsState.AddOrGet(Context);
            _messenger.Unregister(connection); 
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #endregion

        private static Type GetTypeFromName(string typeName)
        {
            var type = MyReflectionHelper.FindOfType(typeof(Project).Assembly, typeName);
            var makeGenericType = MyReflectionHelper.MakeGenericType(typeof(DalUpdateMessage<>), type);
            return makeGenericType;
        }

        private void CallBackToClient(string typeName,object obj)
        {
            _log.Debug("NotificationHub:CallBackToClient " + obj.GetType().FullName);
            Clients.Caller.OnUpdate(typeName,obj);
        }

        
    }
}