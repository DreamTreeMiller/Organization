using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Director : Worker
	{
		public Director(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, string jobTitle, Positions position = Positions.DeptDirector,
						uint salaryBase = 1300)
				: base(FN, LN, DOB, hired, deptID, jobTitle, position, salaryBase)
		{ }

		public override uint Salary
		{
			get { return base.salaryBase; }
			set { base.salaryBase = value > 1300 ? value : 1300; }
		}
	}
}
