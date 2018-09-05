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
		public enum Colors
		{
			Basic = 0,
			Focus = 1,
			Changed = 2,
		}

		private Brush _fore;
		private bool _focused;

		public EventHandler Changed;

		public Cell Cell { get; private set; }

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

		public SudokuPanel(Cell cell)
		{
			_fore = Brushes.Black;
			Font = Font;
			Cell = cell;
			cell.Changed += (s, e) => OnChanged();
		}

		private void OnChanged()
		{
			var h = Changed;
			if (h != null)
				h(this, EventArgs.Empty);
		}

		public void SetColor(Colors color)
		{
			var oldFore = _fore;
			var oldBack = BackColor;
			switch (color)
			{
				case Colors.Basic:
					if (_focused)
					{
						if (Cell.IsFixed)
						{
							BackColor = Color.Blue;
							_fore = Brushes.LightGreen;
						}
						else
						{
							BackColor = Color.Blue;
							_fore = Brushes.White;
						}
					}
					else
					{
						if (Cell.IsFixed)
						{
							BackColor = Color.White;
							_fore = Brushes.DarkGreen;
						}
						else
						{
							BackColor = Color.White;
							_fore = Brushes.Black;
						}
					}
					break;

				case Colors.Changed:
					BackColor = Color.CadetBlue;
					break;

				default:
					BackColor = Color.White;
					_fore = Brushes.Black;
					break;
			}
			if (oldFore != _fore || oldBack != BackColor)
			{
				OnChanged();
			}
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
			_focused = true;
			SetColor(Colors.Basic);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			_focused = false;
			SetColor(Colors.Basic);
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
			}
		}
	}
}
