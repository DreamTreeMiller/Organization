using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Organization : Department
	{
		public Organization (string orgName) 
				: base (orgName, null)
		{ }
		public int MoveEmployee(ulong empID, Department origin, Department destination)
		{
			Worker w = origin.GetEmployee(empID);
			if (w == null) return -1;       // No worker with such ID works in origin dept
			w.DepName = destination.Name;
			destination.AddEmployee(w);
			return 0;
		}
	}
}
