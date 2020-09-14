using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Division : BaseDepartment, IDivision
	{
		public Division(uint deptID)
			: base(deptID)
		{ }

		public Division(string deptName, uint parentDeptID)
			: base (deptName, parentDeptID)
		{ }

		public Division(BaseDepartment d)
			: base(d)
		{ }
	}
}
