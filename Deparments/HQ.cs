using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class HQ : BaseDepartment
	{
		public HQ(uint deptID)
			: base (deptID)
		{
		}

		public HQ(string orgName)
			: base(orgName, 0)
		{ }
	}
}
