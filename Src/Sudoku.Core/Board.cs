using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Strategies;

namespace Sudoku
{
	public class Board
	{
		private Cell[,] _matrix;

		public List<Cell> Cells { get; private set; }
		public List<Segment> Segments { get; private set; }

		public Board(Cell[,] cells)
		{
			if (cells.GetLength(0) != 9 || cells.GetLength(1) != 9)
				throw new ArgumentException("Invalid board definition.");

			_matrix = cells;
			Cells = new List<Cell>();
			Segments = new List<Segment>();

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					Cells.Add(cells[i, j]);
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
					_matrix[x, y].Value = int.Parse(value);
					_matrix[x, y].IsFixed = true;
				}
			}
		}
	}
}
