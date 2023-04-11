using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	[Flags]
	public enum SolvingStrategyType
	{
		None = 0,
		Backtracking = 0x01,
		NakedSingle = 0x02,
		HiddenSingle = 0x04,
		NakedPair = 0x08,
		HiddenPair = 0x10,
		PointingPair = 0x11,
	}

	public interface ISolvingStrategy
	{
		List<Cell> Perform(Board board);
	}

	public static class SolvingStrategy
	{
		public static List<Cell> Perform(SolvingStrategyType strategy, Board board)
		{
			List<Cell> changes;
			switch (strategy)
			{
				case SolvingStrategyType.None: return new List<Cell>();
				case SolvingStrategyType.Backtracking: changes = BacktrackingStrategy.Instance.Perform(board); break;
				case SolvingStrategyType.NakedSingle: changes = NakedSingleStrategy.Instance.Perform(board); break;
				case SolvingStrategyType.HiddenSingle: changes = HiddenSingleStrategy.Instance.Perform(board); break;
				case SolvingStrategyType.NakedPair: changes = NakedPairStrategy.Instance.Perform(board); break;
				case SolvingStrategyType.HiddenPair: changes = HiddenPairStrategy.Instance.Perform(board); break;
				case SolvingStrategyType.PointingPair: changes = PointingPairStrategy.Instance.Perform(board); break;
				default:
					throw new ArgumentOutOfRangeException("action");
			}

			foreach (var cell in changes)
			{
				if (cell.Possibilities == 1 && strategy != SolvingStrategyType.Backtracking)
					cell.Value = Cell.ToValue(cell.Candidates);
			}
			return changes;
		}
	}
}
