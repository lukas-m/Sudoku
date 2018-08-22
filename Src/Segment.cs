using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	public class Segment : List<Cell>
	{
		Dictionary<Cell, Cell> _cells = new Dictionary<Cell, Cell>();

		public int Tag { get; set; }

		public int FreeCells { get; set; }
		
		public new void Add(Cell cell)
		{
			base.Add(cell);
			if (!cell.HasValue)
				FreeCells++;
		}

		public void Register(Cell cell)
		{
			if (cell.Segments.ContainsKey(this))
				throw new ArgumentException("Segment already contains given cell.");
			Add(cell);
			_cells.Add(cell, cell);
			cell.Segments.Add(this, this);
		}

		public Segment Intersect(Segment other)
		{
			Segment intersection = new Segment();
			foreach (var cell in other)
			{
				if (cell.Segments.ContainsKey(this))
					intersection.Add(cell);
			}
			return intersection;
		}

		public new bool Contains(Cell cell)
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
	}
}
