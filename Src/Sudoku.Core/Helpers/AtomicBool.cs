using System;
using System.Threading;

namespace Sudoku
{
	public struct AtomicBool
	{
		private const int NULL = -1;
		private const int FALSE = 0;
		private const int TRUE = 1;

		private int _value;

		public bool Value { get { return Interlocked.CompareExchange(ref _value, NULL, NULL) == TRUE; } }

		public AtomicBool(bool value)
		{
			_value = value ? TRUE : FALSE;
		}

		public bool Set(bool value)
		{
			int val = value ? TRUE : FALSE;
			int comp = value ? FALSE : TRUE;
			return Interlocked.CompareExchange(ref _value, val, comp) == comp;
		}

		public bool SetTrue()
		{
			return Interlocked.CompareExchange(ref _value, TRUE, FALSE) == FALSE;
		}

		public bool SetFalse()
		{
			return Interlocked.CompareExchange(ref _value, FALSE, TRUE) == TRUE;
		}
	}
}
