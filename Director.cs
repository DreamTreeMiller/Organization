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
						Department department, string job, Positions position = Positions.DeptDirector,
						uint salaryBase = 1300)
				: base(FN, LN, DOB, hired, department, job, position, salaryBase)
		{ }

		public override uint Salary => base.salaryBase;

		public override void GetPaid(uint accumulatedSalary)
		{
			base.salaryBase = accumulatedSalary > 1300 ? accumulatedSalary : 1300;
		}
	}
}
