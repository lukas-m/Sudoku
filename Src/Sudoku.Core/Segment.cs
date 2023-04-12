using System;
using System.Collections;
using System.Collections.Generic;

namespace Sudoku
{
	public class Segment : IEnumerable<Cell>
	{
		private readonly List<Cell> _list = new List<Cell>(9);
		private readonly Dictionary<Cell, Cell> _cells = new Dictionary<Cell, Cell>(9);

		public int Count => _list.Count;
		public int FreeCells { get; set; }
		public Candidates Values { get; set; }

		public bool HasValue(int value)
		{
			return ((Values & CandidatesHelper.ToCandidate(value)) != 0);
		}

		public void Add(Cell cell)
		{
			if (cell.Segments.ContainsKey(this))
				throw new ArgumentException("Segment already contains given cell.");

			cell.Segments.Add(this, this);

			_list.Add(cell);
			_cells.Add(cell, cell);
			if (!cell.HasValue)
				FreeCells++;
			else
				Values |= CandidatesHelper.ToCandidate(cell.Value);
		}

		public bool Contains(Cell cell)
		{
			return _cells.ContainsKey(cell);
		}

		public bool IsEqual(Segment other)
		{
			if (Count != other.Count)
				return false;
			foreach (var cell in other)
			{
				if (!_cells.ContainsKey(cell))
					return false;
			}
			return true;
		}

		IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public override string ToString()
		{
			return string.Join(", ", _list);
		}
	}
}
