using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
	public interface IEventUpdateEvent
	{
        void SubscribeToUpdates();
        void UnsubscribeFromUpdates();
	}
    
    public interface IEventUpdateEvent<T> : IEventUpdateEvent
	{
		void OnUpdate(ValueUpdateModel<T> user);
	}
}