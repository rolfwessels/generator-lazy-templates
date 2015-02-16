using System.Collections.Generic;
using System.Linq;

namespace MainSolutionTemplate.Api.SignalR
{
	/// <summary>
	/// The connection mapping.
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	public class ConnectionMapping<T>
	{
		/// <summary>
		/// The connection list.
		/// </summary>
		private readonly Dictionary<T, HashSet<string>> connectionlist = new Dictionary<T, HashSet<string>>();

		/// <summary>
		/// Gets the count.
		/// </summary>
		public int Count
		{
			get
			{
				return this.connectionlist.Count;
			}
		}

		/// <summary>
		/// The add.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="connectionId">
		/// The connection id.
		/// </param>
		public void Add(T key, string connectionId)
		{
			lock (this.connectionlist)
			{
				HashSet<string> connections;
				if (!this.connectionlist.TryGetValue(key, out connections))
				{
					connections = new HashSet<string>();
					this.connectionlist.Add(key, connections);
				}

				lock (connections)
				{
					connections.Add(connectionId);
				}
			}
		}

		/// <summary>
		/// The get connections.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// The <see cref="IEnumerable{T}"/>.
		/// </returns>
		public IEnumerable<string> GetConnections(T key)
		{
			HashSet<string> connections;
			if (this.connectionlist.TryGetValue(key, out connections))
			{
				return connections;
			}

			return Enumerable.Empty<string>();
		}

		/// <summary>
		/// The remove.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="connectionId">
		/// The connection id.
		/// </param>
		public void Remove(T key, string connectionId)
		{
			lock (this.connectionlist)
			{
				HashSet<string> connections;
				if (!this.connectionlist.TryGetValue(key, out connections))
				{
					return;
				}

				lock (connections)
				{
					connections.Remove(connectionId);

					if (connections.Count == 0)
					{
						this.connectionlist.Remove(key);
					}
				}
			}
		}
	}
}