using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Common;
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

namespace MainSolutionTemplate.Api.SignalR.Hubs
{
    
    public class NotificationHub : BaseHub//,IEventUpdateEvent<ProjectModel>
    {
        private const string UpdateGroupName = "project.update.group";
        private readonly ProjectCommonController _projectCommonController;
        private readonly IMessenger _messenger;

        public NotificationHub(ProjectCommonController projectCommonController, IConnectionStateMapping connectionStateMapping , IMessenger messenger)
            : base(connectionStateMapping)
        {
            _projectCommonController = projectCommonController;
            _messenger = messenger;    
        }

        

        #region IEventUpdateEvent Members

        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public async Task SubscribeToUpdates(string typeName)
        {
            var connection = _connectionsState.AddOrGet(Context);
            _messenger.Register<DalUpdateMessage<Project>>(connection,CallBackToClient);
            await Groups.Add(Context.ConnectionId, UpdateGroupName);
        }

        
        [HubAuthorizeActivity(Activity.SubscribeProject)]
        public Task UnsubscribeFromUpdates()
        {
            return Task.Run(() =>
            {
                var connection = _connectionsState.AddOrGet(Context);
                _messenger.Unregister<DalUpdateMessage<Project>>(connection);
            });
        }

        #endregion

        private void CallBackToClient(DalUpdateMessage<Project> dalUpdateMessage)
        {
            Console.Out.WriteLine(dalUpdateMessage);
            var valueUpdateModel = new ValueUpdateModel<Project>(dalUpdateMessage.Value, (UpdateTypeCodes) dalUpdateMessage.UpdateType);
            Console.Out.WriteLine("valueUpdateModel: " + valueUpdateModel.Dump());
            Clients.Caller.OnUpdate(valueUpdateModel);
        }

    }
}