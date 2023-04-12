using System;
using System.Collections.Generic;

namespace Sudoku
{
	public class DictionaryPool
	{
		public static readonly DictionaryPool Shared = new DictionaryPool();

		private readonly Dictionary<string, Queue<IDisposable>> _pool = new Dictionary<string, Queue<IDisposable>>();

		public DisposableDictionary<TKey, TValue> Rent<TKey, TValue>()
		{
			var queue = GetQueue<TKey, TValue>();
			if (queue.Count == 0)
				return new DisposableDictionary<TKey, TValue>(this);

			var dic = queue.Dequeue() as DisposableDictionary<TKey, TValue>;
			dic.IsPooled = false;
			return dic;
		}

		private void Return<TKey, TValue>(DisposableDictionary<TKey, TValue> value)
		{
			if (value.IsPooled)
				throw new InvalidOperationException("Attempt to return value into pool multiple times.");

			var queue = GetQueue<TKey, TValue>();
			queue.Enqueue(value);
			value.Clear();
			value.IsPooled = true;
		}

		private Queue<IDisposable> GetQueue<TKey, TValue>()
		{
			Queue<IDisposable> queue;
			string key = string.Format("{0}@{1}", typeof(TKey).FullName, typeof(TValue).FullName);
			if (!_pool.TryGetValue(key, out queue))
			{
				queue = new Queue<IDisposable>();
				_pool.Add(key, queue);
			}
			return queue;
		}

		public class DisposableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable
		{
			private readonly DictionaryPool _pool;

			internal bool IsPooled = false; // object is not pooled after creation

			public DisposableDictionary(DictionaryPool pool) : base()
			{
				_pool = pool;
			}

			void IDisposable.Dispose()
			{
				if (IsPooled)
					return;
				_pool.Return(this);
			}
		}
	}
}
