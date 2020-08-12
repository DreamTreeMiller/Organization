using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Employee : Worker
	{
		public uint HoursWorked { get; set; }	// recalulated on the last day of the month
		public Employee(string FN, string LN, DateTime DOB,
						DateTime hired,
						string depName, string job, string position,
						uint hourlyRate)
				: base(FN, LN, DOB, hired, depName, job, position, hourlyRate)
		{
			this.HoursWorked = 0;
		}

		// for an employee salaryBase is an hourly base of payment
		public override uint Salary
		{
			get { return (uint)(base.salaryBase * this.HoursWorked); }
		}

		// 
		public override void GetPaid(uint hours)
		{
			this.HoursWorked = hours;
		}

	}
}
