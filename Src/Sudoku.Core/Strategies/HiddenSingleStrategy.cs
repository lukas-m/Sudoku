using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class HiddenSingleStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new HiddenSingleStrategy();

		/// <summary>
		/// If a segment has a candidate in one cell only,
		/// then this cell contains that value.
		/// </summary>
		public List<Cell> Perform(Grid grid)
		{
			List<Cell> changes = new List<Cell>();
			foreach (var seg in grid.Segments)
			{
				for (int i = Cell.MinValue; i < Cell.MaxValue; i++)
				{
					Cell last = null;
					foreach (var cell in seg)
					{
						if (cell.HasValue)
							continue;
						if (!cell.HasCandidate(i))
							continue;
						if (last == null)
							last = cell;
						else
							goto Label_HiddenSingleContinue;
					}
					if (last != null)
					{
						changes.Add(last);
						last.SetSingleCandidate(i);
					}
Label_HiddenSingleContinue:
					continue;
				}
			}
			return changes;
		}
	}
}
