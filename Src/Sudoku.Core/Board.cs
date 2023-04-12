using System;
using System.Collections.Generic;

namespace Sudoku
{
	public class Board
	{
		private Cell[,] Matrix { get; }

		public List<Cell> Cells { get; }
		public List<Segment> Segments { get; }

		public int Rows { get; }
		public int Columns { get; }

		public Board(int rows, int columns)
		{
			if (rows != 9 || columns != 9)
				throw new ArgumentException("Only 9x9 is supported currently.");

			Rows = rows;
			Columns = columns;
			Matrix = new Cell[rows, columns];
			Cells = new List<Cell>();
			Segments = new List<Segment>();

			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < columns; c++)
				{
					var cell = new Cell(r, c);
					Matrix[r, c] = cell;
					Cells.Add(cell);
				}
			}

			for (int i = 0; i < rows; i++)
			{
				Segment row = new Segment();
				Segment column = new Segment();
				for (int j = 0; j < columns; j++)
				{
					row.Add(Matrix[i, j]);
					column.Add(Matrix[j, i]);
				}
				Segments.Add(column);
				Segments.Add(row);
			}

			for (int r = 0; r < 9; r += 3)
			{
				for (int c = 0; c < 9; c += 3)
				{
					Segment square = new Segment();
					for (int k = 0; k < 9; k++)
					{
						square.Add(Matrix[r + (k % 3), c + (k / 3)]);
					}
					Segments.Add(square);
				}
			}
		}

		public void Load(string[] lines)
		{
			char[] separators = new char[] { ' ', '|' };

			int row = -1;
			foreach (var rawline in lines)
			{
				int idx = rawline.IndexOf('#'); // remove comments
				string line = idx < 0 ? rawline.Trim() : rawline.Substring(0, idx).Trim();

				if (line.StartsWith("-") || line.Length == 0)
					continue;

				row++;
				int column = -1;
				var values = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
				foreach (var value in values)
				{
					column++;
					if (value == ".")
						continue;
					Matrix[row, column].Value = int.Parse(value);
					Matrix[row, column].IsFixed = true;
				}
			}
		}

		public Cell GetCell(int row, int column)
		{
			return Matrix[row, column];
		}
	}
}
