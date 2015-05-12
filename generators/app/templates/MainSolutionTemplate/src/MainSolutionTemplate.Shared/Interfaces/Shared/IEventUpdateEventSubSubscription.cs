using System;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IEventUpdateEventSubSubscription<T> 
    {
        Task OnUpdate(Action<ValueUpdateModel<T>> callBack);
        Task UnsubscribeFromUpdates();
    }
}