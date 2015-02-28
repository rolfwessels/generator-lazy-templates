using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
    public interface ISubscribeUpdateModel
    {
        void SubscribeToUpdate();
        void UnsubscribeFromUpdate();
        
    }

    public interface ISubscribeUpdateModel<T> : ISubscribeUpdateModel
    {
        void OnUpdate(ValueUpdateModel<T> updatedModel);
    }
}