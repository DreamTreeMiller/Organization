using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	/// <summary>
	/// Iterface alias of PositionTuple class
	/// </summary>
	public interface IPositionTuple
	{
		/// <summary>
		/// Position type
		/// </summary>
		Positions Pos { get; }

		/// <summary>
		/// Name of position
		/// </summary>
		string PositionName { get; }
	}
}
