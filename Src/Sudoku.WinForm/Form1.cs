using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sudoku.Strategies;

namespace Sudoku
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("ConfigureAwait", "ConfigureAwaitEnforcer:ConfigureAwaitEnforcer", Justification = "<Pending>")]
	public partial class Form1 : Form
	{
		Board _board;
		Dictionary<Cell, SudokuPanel> _cells;
		AtomicBool _refreshDisabled;
		AtomicBool _executing;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_board = new Board(9, 9);
			_cells = new Dictionary<Cell, SudokuPanel>();

			int dy = 0;
			for (int r = 0; r < _board.Rows; r++)
			{
				if (r > 0 && r % 3 == 0)
					dy += 2;

				int dx = 0;
				for (int c = 0; c < _board.Columns; c++)
				{
					if (c > 0 && c % 3 == 0)
						dx += 2;

					var p = new SudokuPanel(_board.GetCell(r, c));
					p.BorderStyle = BorderStyle.FixedSingle;
					p.BackColor = Color.White;
					p.Size = new Size(80, 80);
					p.Location = new Point(dx + c * 80, dy + r * 80);
					p.Font = new Font(FontFamily.GenericSansSerif, 40);
					p.Changed += (s, _) => { if (!_refreshDisabled.Value) ((SudokuPanel)s).Refresh(); };

					panelBoard.Controls.Add(p);
					_cells.Add(p.Cell, p);
				}
			}
		}

		private async void buttonBrowse_Click(object sender, EventArgs e)
		{
			await Execute(() =>
			{
				openFileDialog1.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(textBoxSource.Text));
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					textBoxSource.Text = openFileDialog1.FileName;
					LoadBoard();
				}
			});
		}

		private async void buttonLoad_Click(object sender, EventArgs e)
		{
			await Execute(LoadBoard);
		}

		private void LoadBoard()
		{
			string path = textBoxSource.Text;
			if (!File.Exists(path))
				return;

			_refreshDisabled.SetTrue();
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
				_refreshDisabled.SetFalse();
			}
		}

		private async void buttonReset_Click(object sender, EventArgs e)
		{
			await Execute(Reset);
		}

		private void Reset()
		{
			foreach (var pair in _cells)
			{
				pair.Key.Reset();
				pair.Value.SetColor(SudokuPanel.Colors.Basic);
			}
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

		#region Execute methods

		private Task Execute(Func<List<Cell>> getChangedCells)
		{
			return Execute(async () =>
			{
				foreach (var cell in _cells.Values)
				{
					cell.SetColor(SudokuPanel.Colors.Basic);
				}

				while (true)
				{
					var changed = getChangedCells();
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

		private Task Execute(Action action)
		{
			return Execute(() => { action(); return Task.CompletedTask; });
		}

		private async Task Execute(Func<Task> action)
		{
			if (!_executing.SetTrue())
				return;

			try
			{
				await action();
			}
			finally
			{
				_executing.SetFalse();
			}
		}

		#endregion
	}
}
