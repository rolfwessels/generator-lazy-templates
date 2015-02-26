using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IUserHubEvents
	{
		[HubAuthorizeActivity(Activity.UserSubscribe)]
		void OnUpdate(ValueUpdateModel<UserModel> user);
	}
}