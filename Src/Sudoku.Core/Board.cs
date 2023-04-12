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
		public Cell[,] Matrix { get; }
		public List<Cell> Cells { get; }
		public List<Segment> Segments { get; }

		public Board(int width, int height)
		{
			if (width != 9 || height != 9)
				throw new ArgumentException("Only 9x9 is supported currently.");

			Matrix = new Cell[width, height];
			Cells = new List<Cell>();
			Segments = new List<Segment>();

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					var cell = new Cell();
					Matrix[i, j] = cell;
					Cells.Add(cell);
				}
			}

			for (int i = 0; i < 9; i++)
			{
				Segment column = new Segment();
				for (int j = 0; j < 9; j++)
				{
					column.Register(Matrix[i, j]);
				}
				Segments.Add(column);

				Segment row = new Segment();
				for (int j = 0; j < 9; j++)
				{
					row.Register(Matrix[j, i]);
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
						square.Register(Matrix[i + (k % 3), j + (k / 3)]);
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
					Matrix[x, y].Value = int.Parse(value);
					Matrix[x, y].IsFixed = true;
				}
			}
		}
	}
}
