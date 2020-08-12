using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	abstract class Worker 
	{
		public ulong	ID			{ get;}				// Unique ID - first 6 bytes of Guid
		public string	FirstName	{ get; set; }
		public string	LastName	{ get; set; }
		public DateTime	DateOfBirth { get; set; }
		public DateTime EmployedOn	{ get; set; }
		public string	DepName		{ get; set; }		// Name of department
		public string	Job			{ get; set; }		// responsibilities
		public Positions Position	{ get; set; }       // withing organization President, VP, Head of Division, Dept Director
		protected uint	salaryBase;						// Salary base. for each type of worker meaning is different
		public abstract uint Salary	{ get; }			// Salary - for each type calculated differently
		public abstract void GetPaid(uint calc = 0);	// Method to receive salary. for each type is different

		public Worker(string FN, string LN, DateTime DOB, 
						DateTime hired, 
						string depName, string job, Positions position,
						uint salaryBase = 0)
		{
			// We take first 6 bytes of Guid and compose a unique ulong ID out of them
			byte[] guid = Guid.NewGuid().ToByteArray();
			ID = ((ulong)guid[3] << 8 * 5) |		// This is the oder of bytes 
				 ((ulong)guid[2] << 8 * 4) |		// according to the string format of Guid
				 ((ulong)guid[1] << 8 * 3) |
				 ((ulong)guid[0] << 8 * 2) |
				 ((ulong)guid[5] << 8 * 1) |
				  (ulong)guid[4];
			this.FirstName		= FN;
			this.LastName		= LN;
			this.DateOfBirth	= DOB;
			this.EmployedOn		= hired;
			this.DepName		= depName;
			this.Job			= job;
			this.Position		= position;
			this.salaryBase		= salaryBase;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			Worker w = obj as Worker;
			return this.ID.Equals(w.ID);
		}

		public override int GetHashCode()
		{
			return (int)ID;
		}
	}
}
