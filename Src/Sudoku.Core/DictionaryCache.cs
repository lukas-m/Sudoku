using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	public class DictionaryCache
	{
		Dictionary<Type, Dictionary<Type, Queue<object>>> _cache = new Dictionary<Type, Dictionary<Type, Queue<object>>>();

		public DictionaryCache.Item<TKey, TValue> Get<TKey, TValue>()
		{
			return new Item<TKey, TValue>(this);
		}

		private Dictionary<TKey, TValue> GetInner<TKey, TValue>()
		{
			var queue = GetQueue<TKey, TValue>();
			if (queue.Count == 0)
				return new Dictionary<TKey, TValue>();
			else
				return queue.Dequeue() as Dictionary<TKey, TValue>;
		}

		private void Add<TKey, TValue>(Dictionary<TKey, TValue> value)
		{
			var queue = GetQueue<TKey, TValue>();
			queue.Enqueue(value);
			value.Clear();
		}

		private Queue<object> GetQueue<TKey, TValue>()
		{
			Dictionary<Type, Queue<object>> key;
			if (!_cache.TryGetValue(typeof(TKey), out key))
			{
				key = new Dictionary<Type, Queue<object>>();
				_cache.Add(typeof(TKey), key);
			}

			Queue<object> queue;
			if (!key.TryGetValue(typeof(TValue), out queue))
			{
				queue = new Queue<object>();
				key.Add(typeof(TValue), queue);
			}

			return queue;
		}

		public class Item<TKey, TValue> : IDisposable
		{
			DictionaryCache _owner;

			public Dictionary<TKey, TValue> Dictionary { get; private set; }

			public Item(DictionaryCache owner)
			{
				_owner = owner;
				Dictionary = _owner.GetInner<TKey, TValue>();
			}

			void IDisposable.Dispose()
			{
				_owner.Add(Dictionary);
			}
		}
	}
}
