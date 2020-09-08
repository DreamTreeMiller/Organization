using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	public interface IOrganization
	{
		IPositions PositionsData { get; }
		List<IPositionTuple> Available(IDepartmentDTO dept);
	}
}
