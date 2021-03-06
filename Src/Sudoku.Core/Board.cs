﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	[Flags]
	public enum BoardAction
	{
		None = 0,
		Backtracking = 0x01,
		NakedSingle = 0x02,
		HiddenSingle = 0x04,
		NakedPair = 0x08,
		HiddenPair = 0x10,
		PointingPair = 0x11,
	}

	public class Board
	{
		private Cell[,] _cells;
		private List<Cell> _cellsList;

		private DictionaryCache _cache = new DictionaryCache();

		public List<Segment> Segments { get; private set; }

		public Board(Cell[,] cells)
		{
			if (cells.GetLength(0) != 9 || cells.GetLength(1) != 9)
				throw new ArgumentException("Invalid board definition.");

			_cells = cells;
			_cellsList = new List<Cell>();
			Segments = new List<Segment>();

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					_cellsList.Add(cells[i, j]);
				}

				Segment column = new Segment();
				for (int j = 0; j < 9; j++)
				{
					column.Register(cells[i, j]);
				}
				Segments.Add(column);

				Segment row = new Segment();
				for (int j = 0; j < 9; j++)
				{
					row.Register(cells[j, i]);
				}
				Segments.Add(row);
			}

			for (int i = 0; i < 9; i += 3)
			{
				for (int j = 0; j < 9; j += 3)
				{
					Segment square = new Segment();
					for (int k = 0; k < 9; k++)
					{
						square.Register(cells[i + (k % 3), j + (k / 3)]);
					}
					Segments.Add(square);
				}
			}
		}

		public void Load(string[] lines)
		{
			char[] separators = new char[] { ' ', '|' };

			int x = -1;
			int y = -1;
			foreach (var rawline in lines)
			{
				int idx = rawline.IndexOf('#'); // remove comments
				string line = idx < 0 ? rawline.Trim() : rawline.Substring(0, idx).Trim();

				if (line.StartsWith("-") || line.Length == 0)
					continue;

				x = -1;
				y++;
				var values = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
				foreach (var value in values)
				{
					x++;
					if (value == ".")
						continue;
					_cells[x, y].Value = int.Parse(value);
					_cells[x, y].IsFixed = true;
				}
			}
		}

		public List<Cell> Perform(BoardAction action)
		{
			List<Cell> changes = new List<Cell>();
			switch (action)
			{
				case BoardAction.None: return changes;
				case BoardAction.Backtracking: Backtracking(changes); break;
				case BoardAction.NakedSingle: NakedSingle(changes); break;
				case BoardAction.HiddenSingle: HiddenSingle(changes); break;
				case BoardAction.NakedPair: NakedPair(changes); break;
				case BoardAction.HiddenPair: HiddenPair(changes); break;
				case BoardAction.PointingPair: PointingPair(changes); break;
				default:
					throw new ArgumentOutOfRangeException("action");
			}

			foreach (var cell in changes)
			{
				if (cell.Possibilities == 1 && action != BoardAction.Backtracking)
					cell.Value = Cell.ToValue(cell.Candidates);
			}
			return changes;
		}

		private void Backtracking(List<Cell> changes)
		{
			if (Backtracking())
			{
				foreach (var cell in _cellsList)
				{
					if (!cell.IsFixed)
						changes.Add(cell);
				}
			}
		}

		public bool Backtracking()
		{
			var cells = new List<Cell>();
			foreach (var cell in _cellsList)
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
				foreach (var cell in _cellsList)
				{
					cell.BacktrackingMode = false;
				}
			}
		}

		private bool Backtrack(List<Cell> cells, int idx, Cell testedCell, int value)
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

		/// <summary>
		/// If a cell has single candidate,
		/// then this cell contains that value.
		/// </summary>
		private void NakedSingle(List<Cell> changes)
		{
			foreach (var cell in _cellsList)
			{
				if (cell.Candidates == 0)
					continue;
				if (cell.Possibilities == 1)
					changes.Add(cell);
			}
		}

		/// <summary>
		/// If a segment has a candidate in one cell only,
		/// then this cell contains that value.
		/// </summary>
		private void HiddenSingle(List<Cell> changes)
		{
			foreach (var seg in Segments)
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
		}

		/// <summary>
		/// If two cells in one segment contains the same two candidates only,
		/// then these candidates in other cells can be removed.
		/// </summary>
		private void NakedPair(List<Cell> changes)
		{
			using (var cacheItem = _cache.Get<Candidates, int>())
			{
				var dic = cacheItem.Dictionary;
				foreach (var seg in Segments)
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
		}

		/// <summary>
		/// If two candidates in one segment are contained in the same two cells only,
		/// then other candidates in those cells can be removed.
		/// </summary>
		private void HiddenPair(List<Cell> changes)
		{
			using (var cacheItem = _cache.Get<int, SinglePairInfo>())
			{
				var dic = cacheItem.Dictionary; // cells -> count, Candidates
				foreach (var seg in Segments)
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
		}

		private class SinglePairInfo
		{
			public List<Cell> Cells { get; set; }
			public int Count { get; set; }
			public Candidates Candidates { get; set; }
		}

		/// <summary>
		/// Lets have two segments and its intersection.
		/// If a candidate is contained in the intersection only for one segment,
		/// then this candidate can be removed from the other segment (outside the intersection).
		/// Note:
		/// If intersection is one cell only, then HiddenSingle for this cell can be applied instead.
		/// </summary>
		private void PointingPair(List<Cell> changes)
		{
			throw new NotImplementedException();
		}
	}
}
