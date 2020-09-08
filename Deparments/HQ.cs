using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class HQ : BaseDepartment, IHQ
	{
		public HQ(uint deptID)
			: base (deptID)
		{
		}

		/// <summary>
		/// Creates HQ department and puts 0 to ParentDeptID
		/// </summary>
		/// <param name="orgName"></param>
		public HQ(string orgName)
			: base(orgName, 0)
		{ }
	}
}
