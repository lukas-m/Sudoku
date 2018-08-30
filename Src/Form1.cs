using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
	public partial class Form1 : Form
	{
		Board _board;

		Dictionary<Cell, SudokuPanel> _cells;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Cell[,] cells = new Cell[9, 9];
			_cells = new Dictionary<Cell, SudokuPanel>();

			int dx = 0;
			for (int i = 0; i < 9; i++)
			{
				int dy = 0;
				if (i > 0 && i % 3 == 0)
					dx += 2;
				for (int j = 0; j < 9; j++)
				{
					if (j > 0 && j % 3 == 0)
						dy += 2;

					var p = new SudokuPanel(panelBoard.Refresh);
					p.BorderStyle = BorderStyle.FixedSingle;
					p.BackColor = Color.White;
					p.Size = new Size(80, 80);
					p.Location = new Point(dx + i * 80, dy + j * 80);
					p.Font = new Font(FontFamily.GenericSansSerif, 40);
					p.Cell = new Cell();

					panelBoard.Controls.Add(p);
					cells[i, j] = p.Cell;
					_cells.Add(p.Cell, p);
				}
			}

			_board = new Board(cells);

			//foreach (var line in _board.Lines)
			//{
			//	foreach (var cell in line)
			//	{
			//		_cells[cell].BackColor = Color.Red;
			//	}
			//	panelBoard.Update();
			//	Thread.Sleep(500);
			//	foreach (var cell in line)
			//	{
			//		_cells[cell].BackColor = Color.White;
			//	}
			//}
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			string path = textBoxSource.Text;
			if (!File.Exists(path))
				return;

			Reset();

			var lines = File.ReadAllLines(path);
			_board.Load(lines);
			panelBoard.Refresh();
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			Reset();
			panelBoard.Refresh();
		}

		private void Reset()
		{
			foreach (var pair in _cells)
			{
				pair.Key.Reset();
				pair.Value.BackColor = Color.White;
			}
		}

		private void buttonGenerate_Click(object sender, EventArgs e)
		{
			var rng = new Random();
			foreach (var cell in _cells.Keys)
			{
				if (rng.Next(0, 4) == 0)
				{
					var val = rng.Next(1, 10);
					if (cell.HasCandidate(val))
						cell.Value = val;
				}
			}
			panelBoard.Refresh();
		}

		private void Execute(Func<List<Cell>> action)
		{
			foreach (var cell in _cells.Values)
			{
				cell.BackColor = Color.White;
				cell.Refresh();
			}

			while (true)
			{
				var changed = action();

				foreach (var cell in changed)
				{
					_cells[cell].BackColor = Color.CadetBlue;
					_cells[cell].Refresh();
				}

				if (checkBoxAutoplay.Checked && changed.Count > 0)
					Thread.Sleep(500);
				else
					break;

				foreach (var cell in changed)
				{
					_cells[cell].BackColor = Color.White;
					_cells[cell].Refresh();
				}
			}
		}

		private void buttonOnlyOneNote_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.Single));
		}

		private void buttonOnlyOneInLine_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.HiddenSingle));
		}

		private void buttonNakedPair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.NakedPair));
		}

		private void buttonSinglePair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.SinglePair));
		}

		private void buttonPointingPair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.PointingPair));
		}

		private void buttonXReduction_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.XReduction));
		}
	}
}
