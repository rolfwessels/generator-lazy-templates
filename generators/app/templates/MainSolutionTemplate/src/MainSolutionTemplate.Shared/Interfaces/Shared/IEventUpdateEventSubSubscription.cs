using System;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IEventUpdateEventSubSubscription<T> : IEventUpdateEvent
    {
        void OnUpdate(Action<ValueUpdateModel<T>> callBack);
     
    }
}