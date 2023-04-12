using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class NakedSingleStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new NakedSingleStrategy();

		/// <summary>
		/// If a cell has single candidate,
		/// then this cell contains that value.
		/// </summary>
		public List<Cell> Perform(Grid grid)
		{
			List<Cell> changes = new List<Cell>();
			foreach (var cell in grid.Cells)
			{
				if (cell.Candidates == 0)
					continue;
				if (cell.Possibilities == 1)
					changes.Add(cell);
			}
			return changes;
		}
	}
}
