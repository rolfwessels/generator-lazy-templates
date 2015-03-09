using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IEventUpdateEventSubSubscription<T> : IEventUpdateEvent
    {
        Task OnUpdate(Action<ValueUpdateModel<T>> callBack);
     
    }
}