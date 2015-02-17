using System;
using System.Collections.Concurrent;

namespace MainSolutionTemplate.Core.MessageUtil
{
	public class Messenger
	{
		private static readonly Lazy<Messenger> _massager;
		private ConcurrentDictionary<Type, ConcurrentDictionary<WeakReference, Action<object>>> _dictionary;

		static Messenger()
		{
			_massager = new Lazy<Messenger>(() => new Messenger());
			
		}

		public Messenger()
		{
			_dictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<WeakReference, Action<object>>>();
		}

		public static Messenger Default
		{
			get { return _massager.Value; }
		}

		public void Send<T>(T value)
		{
			ConcurrentDictionary<WeakReference, Action<object>> type;
			if (_dictionary.TryGetValue(typeof (T), out type))
			{
				foreach (var reference in type)
				{
					if (reference.Key.IsAlive)
					{
						reference.Value(value);
					}
					else
					{
						Action<object> removed;
						type.TryRemove(reference.Key, out removed);
					}
				}
			}
		}

		public void Register<T>(object receiver, Action<T> action) where T : class
		{
			WeakReference weakReference = new WeakReference(receiver);
			Action<object> value = (t) => action(t as T);
			var references = _dictionary.GetOrAdd(typeof (T), t => new ConcurrentDictionary<WeakReference, Action<object>>());
			references.AddOrUpdate(weakReference,value,(k,v) => value);
		}

		public void Unregister<T>(object receiver)
		{
			ConcurrentDictionary<WeakReference, Action<object>> type;
			if (_dictionary.TryGetValue(typeof(T), out type))
			{
				Action<object> action;
				foreach (var key in type.Keys)
				{
					if (!key.IsAlive)
					{
						type.TryRemove(key, out action);
						continue;
					}
					if (key.Target == receiver)
					{
						type.TryRemove(key, out action);
					}
				}
				
				
			}
		}

		public void Unregister(object receiver)
		{
			foreach (var type in _dictionary.Values)
			{
				Action<object> action;
				foreach (var key in type.Keys)
				{
					if (!key.IsAlive)
					{
						type.TryRemove(key, out action);
						continue;
					}
					if (key.Target == receiver)
					{
						type.TryRemove(key, out action);
					}
				}
				
				
			}
		}
		
	}
}