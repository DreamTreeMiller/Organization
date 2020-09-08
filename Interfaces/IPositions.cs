using MLM.ActionsBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	/// <summary>
	/// Interface to get positions availabe for certain department, and
	/// name string of specified position
	/// </summary>
	public interface IPositions
	{
		string this[Positions index]
		{ get; }
	}
}
