using System;
using System.Collections.Generic;

namespace Sudoku
{
	[Flags]
	public enum Candidates
	{
		None = 0,
		One = 0x0001,
		Two = 0x0002,
		Three = 0x0004,
		Four = 0x0008,
		Five = 0x0010,
		Six = 0x0020,
		Sven = 0x0040,
		Eight = 0x0080,
		Nine = 0x0100,
	}

	public class Cell
	{
		public const int MinValue = 1;
		public const int MaxValue = 9;
		public const Candidates AllCandidates = (Candidates)0x01FF;

		private int _value;
		private Candidates _candidates;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (value < Cell.MinValue || value > Cell.MaxValue)
					throw new ArgumentException("Invalid value.");
				if (!HasCandidate(value))
					throw new InvalidOperationException("Cannot assign this value.");
				_value = value;
				_candidates = 0;
				foreach (var seg in Segments.Keys)
				{
					foreach (var cell in seg)
					{
						cell.RemoveCandidate(value);
					}
				}
			}
		}

		public Candidates Candidates
		{
			get
			{
				return _candidates;
			}
			set
			{
				if (HasValue)
					throw new InvalidOperationException("Cell has already a value.");
				_candidates = value;
			}
		}

		public bool HasValue { get { return _value != 0; } }

		public Dictionary<Segment, Segment> Segments { get; private set; }

		public Cell()
		{
			Segments = new Dictionary<Segment, Segment>();
			Reset();
		}

		public bool HasCandidate(int value)
		{
			if (HasValue || (Candidates & ToCandidate(value)) == 0)
				return false;
			else
				return true;
		}

		public bool HasSingleCandidate()
		{
			return ToValueInner(Candidates) != 0;
		}

		public void RemoveCandidate(int value)
		{
			if (HasValue)
				return;
			_candidates &= ~ToCandidate(value);
		}

		public void SetCandidate(int value)
		{
			Candidates = Cell.ToCandidate(value);
		}

		public void Reset()
		{
			_value = 0;
			_candidates = Cell.AllCandidates;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1:X4})", Value, Candidates);
		}

		public static Candidates ToCandidate(int value)
		{
			return (Candidates)(1 << (value - 1));
		}

		public static int ToValue(Candidates value)
		{
			CheckSingleValue(value);
			var val = ToValueInner(value);
			if (val == 0)
				throw new InvalidOperationException("Invalid value.");
			return val;
		}

		private static int ToValueInner(Candidates value)
		{
			switch (value)
			{
				case Candidates.One: return 1;
				case Candidates.Two: return 2;
				case Candidates.Three: return 3;
				case Candidates.Four: return 4;
				case Candidates.Five: return 5;
				case Candidates.Six: return 6;
				case Candidates.Sven: return 7;
				case Candidates.Eight: return 8;
				case Candidates.Nine: return 9;
				default: return 0;
			}
		}

		public static string ToString(Candidates value)
		{
			return ToValue(value).ToString();
		}

		private static void CheckSingleValue(Candidates value)
		{
			if ((Cell.AllCandidates & value) != value)
				throw new ArgumentException("Single value required.");
		}
	}
}
