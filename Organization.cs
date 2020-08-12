using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Organization : Department
	{
		public static string OrgName { get; set; }	// Organization name
		public static List<Department> Divisions { get; set; }
		//public List<Worker> DepartmentStaff() { }
		public int MoveEmployee(ulong empID, Department origin, Department destination)
		{
			Worker w = origin.GetEmployee(empID);
			if (w == null) return -1;       // No worker with such ID works in origin dept
			w.DepName = destination.DepName;
			destination.AddEmployee(w);
			return 0;
		}


	}
}
