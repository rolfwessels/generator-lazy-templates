using System;

namespace MainSolutionTemplate.Core.MessageUtil
{
	public interface IMessenger
	{
		void Send<T>(T value);
		void Register<T>(object receiver, Action<T> action) where T : class;
        void Register(Type type, object receiver, Action<object> callBackToClient);
		void Unregister<T>(object receiver);
	    void Unregister(Type type, object receiver);
	    void Unregister(object receiver);
	}
}