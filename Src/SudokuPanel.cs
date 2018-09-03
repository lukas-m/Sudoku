using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
	public class SudokuPanel : Panel
	{
		private Brush _fore;
		private Action _refresh;

		public Cell Cell { get; set; }

		public Font CandidateFont { get; private set; }
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				CandidateFont = new Font(value.FontFamily, value.Size / 3);
			}
		}

		public SudokuPanel(Action refresh)
		{
			Font = Font;
			_refresh = refresh;
			_fore = Brushes.Black;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (Cell.HasValue)
			{
				e.Graphics.DrawString(Cell.Value.ToString(), Font, _fore, new RectangleF(24, 10, ClientSize.Width, ClientSize.Height), StringFormat.GenericTypographic);
			}
			else
			{
				if (Cell.Candidates == 0 && !Focused)
					BackColor = Color.Red;
				int x = 0;
				int y = 3 - ClientSize.Height / 3;
				for (int i = 0; i < 9; i++)
				{
					if ((i % 3) == 0)
					{
						x = 6;
						y += ClientSize.Height / 3;
					}
					else
					{
						x += ClientSize.Width / 3;
					}
					if (!Cell.HasCandidate(i + 1))
						continue;
					e.Graphics.DrawString((i + 1).ToString(), CandidateFont, _fore, new RectangleF(x, y, ClientSize.Width / 3, ClientSize.Height / 3), StringFormat.GenericTypographic);
				}
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Focus();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			BackColor = Color.Blue;
			_fore = Brushes.White;
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			BackColor = Color.White;
			_fore = Brushes.Black;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			int value = 0;
			if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
			{
				value = (e.KeyCode - Keys.D0);
			}
			if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9)
			{
				value = (e.KeyCode - Keys.NumPad0);
			}
			if (value > 0 && Cell.HasCandidate(value))
			{
				Cell.Value = value;
				_refresh();
			}
		}
	}
}
