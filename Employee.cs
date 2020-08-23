using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Employee : Worker
	{
		public int HoursWorked { get; set; }	// recalulated on the last day of the month
		public Employee(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, string jobTitle, Positions position = Positions.Employee,
						int hourlyRate = 12)
				: base(FN, LN, DOB, hired, deptID, jobTitle, position, hourlyRate)
		{
			this.HoursWorked = 22 * 8;
		}

		// for an employee salaryBase is an hourly base of payment
		public override int Salary
		{
			get { return base.salaryBase * this.HoursWorked; }
			set { this.HoursWorked = value; }
		}
	}
}
