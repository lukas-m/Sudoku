using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class PointingPairStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new PointingPairStrategy();

		/// <summary>
		/// Lets have two segments and its intersection.
		/// If a candidate is contained in the intersection only for one segment,
		/// then this candidate can be removed from the other segment (outside the intersection).
		/// Note:
		/// If intersection is one cell only, then HiddenSingle for this cell can be applied instead.
		/// </summary>
		public List<Cell> Perform(Grid grid)
		{
			throw new NotImplementedException("Pointing Pair strategy is not implemented yet.");
		}
	}
}
