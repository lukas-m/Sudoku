using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Sudoku
{
	public partial class Form1 : Form
	{
		Board _board;
		Dictionary<Cell, SudokuPanel> _cells;
		bool _refreshDisabled;

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

					var p = new SudokuPanel(new Cell());
					p.BorderStyle = BorderStyle.FixedSingle;
					p.BackColor = Color.White;
					p.Size = new Size(80, 80);
					p.Location = new Point(dx + i * 80, dy + j * 80);
					p.Font = new Font(FontFamily.GenericSansSerif, 40);
					p.Changed += PanelChanged;

					panelBoard.Controls.Add(p);
					cells[i, j] = p.Cell;
					_cells.Add(p.Cell, p);
				}
			}

			_board = new Board(cells);
		}

		private void PanelChanged(object sender, EventArgs e)
		{
			if (_refreshDisabled)
				return;
			var p = (SudokuPanel)sender;
			p.Refresh();
		}

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(textBoxSource.Text));
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				textBoxSource.Text = openFileDialog1.FileName;
				LoadBoard();
			}
		}

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			LoadBoard();
		}

		private void LoadBoard()
		{
			string path = textBoxSource.Text;
			if (!File.Exists(path))
				return;

			_refreshDisabled = true;
			try
			{
				Reset();

				var lines = File.ReadAllLines(path);
				_board.Load(lines);

				foreach (var cell in _cells.Values)
				{
					cell.SetColor(SudokuPanel.Colors.Basic);
				}

				panelBoard.Refresh();
			}
			finally
			{
				_refreshDisabled = false;
			}
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void Reset()
		{
			foreach (var pair in _cells)
			{
				pair.Key.Reset();
				pair.Value.SetColor(SudokuPanel.Colors.Basic);
			}
		}

		private void Execute(Func<List<Cell>> action)
		{
			foreach (var cell in _cells.Values)
			{
				cell.SetColor(SudokuPanel.Colors.Basic);
			}

			while (true)
			{
				var changed = action();
				if (changed == null)
					return;

				foreach (var cell in changed)
				{
					_cells[cell].SetColor(SudokuPanel.Colors.Changed);
				}

				if (checkBoxAutoplay.Checked && changed.Count > 0)
					Thread.Sleep(500);
				else
					return;

				foreach (var cell in changed)
				{
					_cells[cell].SetColor(SudokuPanel.Colors.Basic);
				}
			}
		}

		private void buttonBacktracking_Click(object sender, EventArgs e)
		{
			Execute(() =>
			{
				bool found = _board.Backtracking();
				if (found)
					panelBoard.Refresh();
				else
					MessageBox.Show("No solution found.");
				return null;
			});
		}

		private void buttonNakedSingle_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.NakedSingle));
		}

		private void buttonHiddenSingle_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.HiddenSingle));
		}

		private void buttonNakedPair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.NakedPair));
		}

		private void buttonHiddenPair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.HiddenPair));
		}

		private void buttonPointingPair_Click(object sender, EventArgs e)
		{
			Execute(() => _board.Perform(BoardAction.PointingPair));
		}
	}
}
