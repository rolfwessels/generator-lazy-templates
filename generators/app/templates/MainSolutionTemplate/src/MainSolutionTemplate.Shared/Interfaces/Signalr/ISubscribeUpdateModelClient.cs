using System;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Signalr
{
    public interface ISubscribeUpdateModelClient<T> 
    {
        void SubscribeToUpdate(Action<ValueUpdateModel<T>> callBack);
        void UnsubscribeFromUpdate();
        
    }
}