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
					   uint deptID, string jobTitle, int salaryBase=500) 
				: base (FN, LN, DOB, hired, deptID, jobTitle, Positions.Intern, salaryBase)
		{
		}
		public override int Salary
		{
			get { return salaryBase; }
			set { }		// Do nothing becase salary is already set by the constructor
		}
	}
}
