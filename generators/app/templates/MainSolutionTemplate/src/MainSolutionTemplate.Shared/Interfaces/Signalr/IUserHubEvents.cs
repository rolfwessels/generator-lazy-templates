using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
	public interface IUserHubEvents
	{
		void OnUpdate(ValueUpdateModel<UserModel> user);
	}
}