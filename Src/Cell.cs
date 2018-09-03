using System;
using System.Collections.Generic;

namespace Sudoku
{
	public class Cell
	{
		public const int MinValue = 1;
		public const int MaxValue = 9;
		public const Candidates AllCandidates = (Candidates)0x01FF;

		private int _value;
		private Candidates _candidates;

		public int Possibilities { get; private set; }

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (value < Cell.MinValue || value > Cell.MaxValue)
					throw new ArgumentOutOfRangeException("Invalid value.");
				if (!HasCandidate(value))
					throw new InvalidOperationException("Cannot assign this value.");
				_value = value;
				Candidates = 0;
				Possibilities = 0;
				foreach (var seg in Segments.Keys)
				{
					seg.FreeCells--;
					seg.Values |= CandidatesHelper.ToCandidate(value);
					foreach (var cell in seg)
					{
						cell.RemoveSingleCandidate(value);
					}
				}
			}
		}

		public Candidates Candidates
		{
			get { return _candidates; }
			private set { _candidates = value; }
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
			if (HasValue || (Candidates & CandidatesHelper.ToCandidate(value)) == 0)
				return false;
			else
				return true;
		}

		public void RemoveSingleCandidate(int value)
		{
			if (value < Cell.MinValue || value > Cell.MaxValue)
				throw new ArgumentOutOfRangeException("Invalid value.");
			if (HasValue)
				return;
			if (!HasCandidate(value))
				return;
			Candidates &= ~CandidatesHelper.ToCandidate(value);
			Possibilities--;
		}

		public void RemoveCandidates(Candidates candidates)
		{
			if (HasValue)
				return;
			Candidates &= ~candidates;
			Possibilities = CandidatesHelper.Count(Candidates);
		}

		public void SetSingleCandidate(int value)
		{
			if (value < Cell.MinValue || value > Cell.MaxValue)
				throw new ArgumentOutOfRangeException("Invalid value.");
			if (HasValue)
				throw new InvalidOperationException("Cell has already a value.");
			Candidates = CandidatesHelper.ToCandidate(value);
			Possibilities = 1;
		}

		public void SetCandidates(Candidates candidates)
		{
			if (HasValue)
				throw new InvalidOperationException("Cell has already a value.");
			if ((Candidates | candidates) != Candidates)
				throw new InvalidOperationException("Cannot add new candidates.");
			Candidates = candidates;
			Possibilities = CandidatesHelper.Count(Candidates);
		}

		public void Reset()
		{
			if (HasValue)
			{
				foreach (var seg in Segments.Keys)
				{
					seg.FreeCells++;
					seg.Values &= ~CandidatesHelper.ToCandidate(Value);
				}
			}
			_value = 0;
			Candidates = Cell.AllCandidates;
			Possibilities = CandidatesHelper.Count(Candidates);
		}

		public static int ToValue(Candidates value)
		{
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

		public override string ToString()
		{
			return string.Format("{0} ({1})", Value, Candidates);
		}
	}
}
