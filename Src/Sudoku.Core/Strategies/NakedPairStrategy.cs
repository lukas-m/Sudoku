using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class NakedPairStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new NakedPairStrategy();

		/// <summary>
		/// If two cells in one segment contains the same two candidates only,
		/// then these candidates in other cells can be removed.
		/// </summary>
		public List<Cell> Perform(Grid grid)
		{
			List<Cell> changes = new List<Cell>();
			using (var dic = DictionaryPool.Shared.Rent<Candidates, int>())
			{
				foreach (var seg in grid.Segments)
				{
					if (seg.FreeCells < 3)
						continue;

					dic.Clear();
					foreach (var cell in seg)
					{
						if (cell.HasValue)
							continue;
						if (dic.ContainsKey(cell.Candidates))
							dic[cell.Candidates]++;
						else
							dic[cell.Candidates] = 1;
					}

					foreach (var pair in dic)
					{
						if (pair.Value < 2 || pair.Value >= seg.FreeCells)
							continue;
						if (CandidatesHelper.Count(pair.Key) != pair.Value)
							continue;
						foreach (var cell in seg)
						{
							if (cell.HasValue || cell.Candidates == pair.Key)
								continue;
							var old = cell.Candidates;
							cell.RemoveCandidates(pair.Key);
							if (cell.Candidates != old)
								changes.Add(cell);
						}
					}
				}
			}
			return changes;
		}
	}
}
