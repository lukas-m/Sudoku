using System;
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
		Single = 0x1,
		HiddenSingle = 0x2,
		NakedPair = 0x4,
		SinglePair = 0x8,
		PointingPair = 0x10,
		XReduction = 0x20,
	}

	public class Board
	{
		private Cell[,] _cells;
		private List<Cell> _cellsList;

		private Queue<Dictionary<Candidates, int>> _cache_Candidates_int = new Queue<Dictionary<Candidates, int>>();

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

		private Dictionary<Candidates, int> GetDictionary_Candidates_int()
		{
			if (_cache_Candidates_int.Count == 0)
				return new Dictionary<Candidates, int>();
			else
				return _cache_Candidates_int.Dequeue();
		}

		private void ReturnDictionary_Candidates_int(Dictionary<Candidates, int> dic)
		{
			dic.Clear();
			_cache_Candidates_int.Enqueue(dic);
		}

		public void Load(string[] lines)
		{
			char[] separators = new char[] { ' ', '|' };

			int x = -1;
			int y = -1;
			foreach (var line in lines)
			{
				if (line.Contains('-') || line.Length < 9)
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
				}
			}
		}

		public List<Cell> Perform(BoardAction action)
		{
			List<Cell> changes = new List<Cell>();
			switch (action)
			{
				case BoardAction.None: return changes;
				case BoardAction.Single: PerformSingle(changes); break;
				case BoardAction.HiddenSingle: PerformHiddenSingle(changes); break;
				case BoardAction.NakedPair: NakedPair(changes); break;
				case BoardAction.SinglePair: SinglePair(changes); break;
				case BoardAction.PointingPair: PointingPair(changes); break;
				case BoardAction.XReduction: XReduction(changes); break;
				default:
					throw new ArgumentOutOfRangeException("action");
			}

			foreach (var cell in changes)
			{
				if (cell.Possibilities == 1)
					cell.Value = Cell.ToValue(cell.Candidates);
			}
			return changes;
		}

		/// <summary>
		/// . . x | - - - | x . . 
		/// . . . | x x x | . . . 
		/// . x . | - - - | . x . 
		/// </summary>
		private void XReduction(List<Cell> changes)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Lets have two segments and its intersection.
		/// If a candidate is contained in the intersection only for one segment,
		/// then this candidate can be removed from the other segment (outside the intersection).
		/// </summary>
		private void PointingPair(List<Cell> changes)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// If a cell has single candidate,
		/// then this cell contains that value.
		/// </summary>
		private void PerformSingle(List<Cell> changes)
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
		private void PerformHiddenSingle(List<Cell> changes)
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
			foreach (var seg in Segments)
			{
				if (seg.FreeCells < 3)
					continue;

				var dic = GetDictionary_Candidates_int();
				foreach (var cell in seg)
				{
					if (cell.HasValue)
						continue;
					if (dic.ContainsKey(cell.Candidates))
						dic[cell.Candidates]++;
					else
						dic[cell.Candidates] = 1;
				}

				for (int i = 2; i < seg.FreeCells; i++)
				{
					foreach (var pair in dic)
					{
						if (pair.Value != i || Cell.CountPossibilities(pair.Key) != i)
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
		private void SinglePair(List<Cell> changes)
		{
			foreach (var seg in Segments)
			{
				var xxx = new List<Segment>();
				for (int i = Cell.MinValue; i <= Cell.MaxValue; i++)
				{
					var s = new Segment();
					s.Tag = i;
					foreach (var cell in seg)
					{
						if (cell.HasCandidate(i))
							s.Add(cell);
					}
					xxx.Add(s);
				}

				for (int len = 2; len < 3; len++)
				{
					var correctLength = new List<Segment>();
					foreach (var s in xxx)
					{
						if (s.Count == len)
							correctLength.Add(s);
					}

					var matchingSegments = new List<Segment>();
					for (int i = 0; i < correctLength.Count - 1; i++)
					{
						for (int j = i + 1; j < correctLength.Count; j++)
						{
							if (correctLength[i].IsEqual(correctLength[j]))
							{
								matchingSegments.Add(correctLength[i]);
								matchingSegments.Add(correctLength[j]);
							}
						}
					}


				}
			}
		}
	}
}
