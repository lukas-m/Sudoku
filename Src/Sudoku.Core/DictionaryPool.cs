using System;
using System.Collections.Generic;

namespace Sudoku
{
	public class DictionaryPool
	{
		Dictionary<string, Queue<IDisposable>> _pool = new Dictionary<string, Queue<IDisposable>>();

		public DisposableDictionary<TKey, TValue> Get<TKey, TValue>()
		{
			var queue = GetQueue<TKey, TValue>();
			if (queue.Count == 0)
				return new DisposableDictionary<TKey, TValue>(this);
			else
				return queue.Dequeue() as DisposableDictionary<TKey, TValue>;
		}

		private void Add<TKey, TValue>(DisposableDictionary<TKey, TValue> value)
		{
			var queue = GetQueue<TKey, TValue>();
			queue.Enqueue(value);
			value.Clear();
		}

		private Queue<IDisposable> GetQueue<TKey, TValue>()
		{
			Queue<IDisposable> queue;
			string key = string.Format("{0}@@{1}", typeof(TKey).FullName, typeof(TValue).FullName);
			if (!_pool.TryGetValue(key, out queue))
			{
				queue = new Queue<IDisposable>();
				_pool.Add(key, queue);
			}
			return queue;
		}

		public class DisposableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable
		{
			DictionaryPool _owner;

			public DisposableDictionary(DictionaryPool owner) : base()
			{
				_owner = owner;
			}

			void IDisposable.Dispose()
			{
				_owner.Add(this);
			}
		}
	}
}
