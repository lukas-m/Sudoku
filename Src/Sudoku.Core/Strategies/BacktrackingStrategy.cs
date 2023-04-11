﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Strategies
{
	public class BacktrackingStrategy : ISolvingStrategy
	{
		internal static readonly ISolvingStrategy Instance = new BacktrackingStrategy();

		public List<Cell> Perform(Board board)
		{
			List<Cell> changes = new List<Cell>();
			Backtracking(board, changes);
			return changes;
		}

		private static void Backtracking(Board board, List<Cell> changes)
		{
			if (Backtrack(board))
			{
				foreach (var cell in board.Cells)
				{
					if (!cell.IsFixed)
						changes.Add(cell);
				}
			}
		}

		public static bool Backtrack(Board board)
		{
			var cells = new List<Cell>();
			foreach (var cell in board.Cells)
			{
				cell.BacktrackingMode = true;
				cells.Add(cell);
			}
			cells = new List<Cell>(cells.OrderBy(c => c.Possibilities));

			try
			{
				for (int c = 0; c < cells.Count; c++)
				{
					var cell = cells[c];
					if (!cell.HasValue)
					{
						for (int i = Cell.MinValue; i <= Cell.MaxValue; i++)
						{
							if (cell.HasCandidate(i) && Backtrack(cells, c, cell, i))
								return true;
						}
						return false;
					}
				}
				return true;
			}
			finally
			{
				foreach (var cell in board.Cells)
				{
					cell.BacktrackingMode = false;
				}
			}
		}

		private static bool Backtrack(List<Cell> cells, int idx, Cell testedCell, int value)
		{
			foreach (var seg in testedCell.Segments.Keys)
			{
				foreach (var tc in seg)
				{
					if (tc.Value == value)
						return false;
				}
			}

			testedCell.Value = value;

			int c = idx + 1;
			if (c == cells.Count)
				return true;

			var cell = cells[c];
			for (int i = Cell.MinValue; i <= Cell.MaxValue; i++)
			{
				if (cell.HasCandidate(i) && Backtrack(cells, c, cell, i))
					return true;
			}

			testedCell.Value = Cell.EmptyValue;
			return false;
		}
	}
}
