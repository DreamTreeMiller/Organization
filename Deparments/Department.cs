using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Department : BaseDepartment
	{
		public Department(uint deptID)
			: base(deptID)
		{ }
		public Department(string deptName, uint parentDeptID)
			: base(deptName, parentDeptID)
		{ }
	}
}
