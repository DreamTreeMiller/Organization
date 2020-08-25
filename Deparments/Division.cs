using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Division : BaseDepartment
	{
		public Division(uint deptID)
			: base(deptID)
		{ }

		public Division(string deptName, uint parentDeptID)
			: base (deptName, parentDeptID)
		{ }
	}
}
