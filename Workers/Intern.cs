using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Intern : Worker, IWorker, IIntern
	{
		public int Age
		{
			get => (int)((DateTime.Now - DateOfBirth).TotalDays / 365.25);
		}

		public override int Salary { get; set; }

		public Intern (string FN, string LN, DateTime DOB,
					   DateTime hired,
					   uint deptID, string jobTitle, int salary=500) 
				: base (FN, LN, DOB, hired, deptID, jobTitle, Positions.Intern)
		{
			Salary = salary;
		}

		/// <summary>
		/// Constructor to change type of worker to Intern
		/// ID and common fields will be copied to new instance of the Intern class 
		/// position related and Salary properties need to be updated additionally
		/// after creating new instance of the Intern
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Intern(IWorkerDTO w)
			: base(w.ID)
		{
			this.FirstName		= w.FirstName;
			this.LastName		= w.LastName;
			this.DateOfBirth	= w.DateOfBirth;
			this.EmployedOn		= w.EmployedOn;
			this.DeptID			= w.DeptID;
		}

		public Intern(Worker w)
			: base(w.ID)
		{
			this.FirstName		= w.FirstName;
			this.LastName		= w.LastName;
			this.DateOfBirth	= w.DateOfBirth;
			this.EmployedOn		= w.EmployedOn;
			this.DeptID			= w.DeptID;
			this.Position		= Positions.Employee;
			this.PositionTitle	= "Intern";
			this.Salary			= 500;
		}

		//public object Clone()
		//{
		//	return (Intern)this.MemberwiseClone();
		//}
	}
}
