using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sudoku.Strategies;

namespace Sudoku
{
	public partial class Form1 : Form
	{
		Board _board;
		Dictionary<Cell, SudokuPanel> _cells;
		bool _refreshDisabled;
		int _executing;

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

		private async Task Execute(Func<Task> action)
		{
			if (System.Threading.Interlocked.CompareExchange(ref _executing, 1, 0) == 1)
				return;

			try
			{
				await action();
			}
			finally
			{
				System.Threading.Interlocked.CompareExchange(ref _executing, 0, 1);
			}
		}

		private Task Execute(Func<List<Cell>> action)
		{
			return Execute(async () =>
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
						await Task.Delay(500);
					else
						return;

					foreach (var cell in changed)
					{
						_cells[cell].SetColor(SudokuPanel.Colors.Basic);
					}
				}
			});
		}

		private async void buttonBrowse_Click(object sender, EventArgs e)
		{
			await Execute(async () =>
			{
				openFileDialog1.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(textBoxSource.Text));
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					textBoxSource.Text = openFileDialog1.FileName;
					await LoadBoard();
				}
			});
		}

		private async void buttonLoad_Click(object sender, EventArgs e)
		{
			await Execute((Func<Task>)LoadBoard);
		}

		private async Task LoadBoard()
		{
			string path = textBoxSource.Text;
			if (!File.Exists(path))
				return;

			_refreshDisabled = true;
			try
			{
				await Reset();

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

		private async void buttonReset_Click(object sender, EventArgs e)
		{
			await Execute((Func<Task>)Reset);
		}

		private Task Reset()
		{
			foreach (var pair in _cells)
			{
				pair.Key.Reset();
				pair.Value.SetColor(SudokuPanel.Colors.Basic);
			}
			return Task.CompletedTask;
		}

		private async void buttonBacktracking_Click(object sender, EventArgs e)
		{
			await Execute(() =>
			{
				bool found = BacktrackingStrategy.Backtrack(_board);
				if (found)
					panelBoard.Refresh();
				else
					MessageBox.Show("No solution found.");
				return Task.CompletedTask;
			});
		}

		private async void buttonNakedSingle_Click(object sender, EventArgs e)
		{
			await Execute(() => SolvingStrategy.Perform(SolvingStrategyType.NakedSingle, _board));
		}

		private async void buttonHiddenSingle_Click(object sender, EventArgs e)
		{
			await Execute(() => SolvingStrategy.Perform(SolvingStrategyType.HiddenSingle, _board));
		}

		private async void buttonNakedPair_Click(object sender, EventArgs e)
		{
			await Execute(() => SolvingStrategy.Perform(SolvingStrategyType.NakedPair, _board));
		}

		private async void buttonHiddenPair_Click(object sender, EventArgs e)
		{
			await Execute(() => SolvingStrategy.Perform(SolvingStrategyType.HiddenPair, _board));
		}

		private async void buttonPointingPair_Click(object sender, EventArgs e)
		{
			await Execute(() => SolvingStrategy.Perform(SolvingStrategyType.PointingPair, _board));
		}
	}
}
