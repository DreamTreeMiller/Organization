using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Intern : Worker
	{
		public Intern (string FN, string LN, DateTime DOB,
					   DateTime hired,
					   Department department, string job, uint salaryBase=500) 
				: base (FN, LN, DOB, hired, department, job, Positions.Intern, salaryBase)
		{
		}
		public override uint Salary
		{
			get { return base.salaryBase; }
		}

		public override void GetPaid(uint calc = 0)
		{
			// Do nothing becase salary is already set up through constructor
		}
	}
}
