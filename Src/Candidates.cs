using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	[Flags]
	public enum Candidates
	{
		None = 0,
		One = 0x0001,
		Two = 0x0002,
		Three = 0x0004,
		Four = 0x0008,
		Five = 0x0010,
		Six = 0x0020,
		Sven = 0x0040,
		Eight = 0x0080,
		Nine = 0x0100,
	}

	public static class CandidatesHelper
	{
		private static readonly int[] Counts = new int[0x200];

		static CandidatesHelper()
		{
			for (int i = 1, r = 0; i < Counts.Length; i++)
			{
				Counts[i] = Counts[r] + 1;
				if (r << 1 == i - 1)
					r = 0;
				else
					r++;
			}
		}

		public static int Count(Candidates candidates)
		{
			return Counts[(int)candidates];
		}

		public static int BitCount(int value)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException("value");

			if (value < Counts.Length)
				return Counts[value];

			int count = 0;
			while (value > 0)
			{
				count += Counts[value & 0x1FF];
				value >>= 9;
			}
			return count;
		}
	}
}
