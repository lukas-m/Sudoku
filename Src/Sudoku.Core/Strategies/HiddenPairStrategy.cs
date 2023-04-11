using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class HiddenPairStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new HiddenPairStrategy();

		/// <summary>
		/// If two candidates in one segment are contained in the same two cells only,
		/// then other candidates in those cells can be removed.
		/// </summary>
		public List<Cell> Perform(Board board)
		{
			List<Cell> changes = new List<Cell>();
			using (var dic = DictionaryPool.Shared.Rent<int, SinglePairInfo>())
			{
				// cells -> count, Candidates
				foreach (var seg in board.Segments)
				{
					if (seg.FreeCells < 3)
						continue;

					dic.Clear();
					for (int i = Cell.MinValue; i <= Cell.MaxValue; i++)
					{
						if (seg.HasValue(i))
							continue;

						List<Cell> cells = new List<Cell>();
						int cellMask = 0;
						int mask = 1;
						foreach (var cell in seg)
						{
							if (cell.HasCandidate(i))
							{
								cellMask |= mask;
								cells.Add(cell);
							}
							mask <<= 1;
						}

						SinglePairInfo info;
						if (dic.TryGetValue(cellMask, out info))
						{
							info.Count++;
							info.Candidates |= CandidatesHelper.ToCandidate(i);
						}
						else
						{
							dic[cellMask] = new SinglePairInfo() { Cells = cells, Count = 1, Candidates = CandidatesHelper.ToCandidate(i) };
						}
					}

					foreach (var info in dic.Values)
					{
						if (info.Count < 2 || info.Cells.Count != info.Count)
							continue;
						foreach (var cell in info.Cells)
						{
							if (cell.Possibilities != info.Count)
							{
								cell.SetCandidates(info.Candidates);
								changes.Add(cell);
							}
						}
					}
				}
			}
			return changes;
		}

		private class SinglePairInfo
		{
			public List<Cell> Cells { get; set; }
			public int Count { get; set; }
			public Candidates Candidates { get; set; }
		}
	}
}
