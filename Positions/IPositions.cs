using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	/// <summary>
	/// Interface to get positions availabe for certain department, and
	/// name string of specified position
	/// </summary>
	public interface IPositions
	{
		List<IPositionTuple> Available(Organization org, BaseDepartment d);

		string this[Positions index]
		{ get; }
	}
}
