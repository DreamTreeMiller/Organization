using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	abstract class Worker
	{
		public string	ID			{ get;}				// Unique ID
		public string	FirstName	{ get; set; }
		public string	LastName	{ get; set; }
		public DateTime	DateOfBirth { get; private set; }
		public DateTime EmployedOn	{ get; set; }
		public string	DepName		{ get; set; }		// Name of department
		public string	Job			{ get; set; }		// responsibilities
		public string	Position	{ get; set; }       // withing organization President, VP, Head of Division, Dept Director
		protected uint	salaryBase;						// Salary base. for each type is different
		public abstract uint Salary	{ get; }			// Salry - for each type calculated differently
		public abstract void GetPaid(uint calc = 0);	// Method to receive salary. for each type is different

		public Worker(string FN, string LN, DateTime DOB, 
						DateTime hired, 
						string depName, string job, string position,
						uint salaryBase = 0)
		{
			ID = Guid.NewGuid().ToString().Substring(0, 7);
			this.FirstName = FN;
			this.LastName = LN;
			this.DateOfBirth = DOB;
			this.EmployedOn = hired;
			this.DepName = depName;
			this.Job = job;
			this.Position = position;
			this.salaryBase = salaryBase;
		}

	}
}
