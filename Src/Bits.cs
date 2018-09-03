using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	public static class Bits
	{
		private static readonly int[] Counts = new int[0x200];

		static Bits()
		{
			for (int i = 1, r = 0; i < Counts.Length; i++)
			{
				Counts[i] = Counts[r] + 1;
				if ((r << 1) == (i - 1))
					r = 0;
				else
					r++;
			}
		}

		public static int Count(int value)
		{
			if (value == 0)
				return 0;

			if (value > 0 && value < Counts.Length)
				return Counts[value];

			int count;
			if (value < 0)
			{
				count = 1;
				value &= 0x7FFFFFFF;
			}
			else
			{
				count = 0;
			}

			while (value > 0)
			{
				count += Counts[value & 0x1FF];
				value >>= 9;
			}
			return count;
		}
	}
}
