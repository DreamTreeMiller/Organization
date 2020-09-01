using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public interface IPositionTuple
	{
		Positions Pos { get; }
		string PositionName { get; }
	}
}
