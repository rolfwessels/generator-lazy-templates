using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
	public interface IEventUpdateEvent
	{
        Task SubscribeToUpdates();
        Task UnsubscribeFromUpdates();
	}
    
    public interface IEventUpdateEvent<T> : IEventUpdateEvent
	{
		Task OnUpdate(ValueUpdateModel<T> user);
	}
}