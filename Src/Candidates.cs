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
}
