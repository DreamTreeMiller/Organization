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
					  string depName, string job, string position, 
					  uint salaryBase=500) 
				: base (FN, LN, DOB, hired, depName, job, position, salaryBase)
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
