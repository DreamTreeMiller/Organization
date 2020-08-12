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
						string depName, string job, Positions position = Positions.DeptDirector,
						uint salary = 1300)
				: base(FN, LN, DOB, hired, depName, job, position, salary)
		{ }

		public override uint Salary => base.salaryBase;

		public override void GetPaid(uint accumulatedSalary)
		{
			base.salaryBase = accumulatedSalary;
		}
	}
}
