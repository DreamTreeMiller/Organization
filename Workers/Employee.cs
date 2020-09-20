using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Employee : Worker, IWorker, IEmployee
	{
		public int Age
		{
			get => (int)((DateTime.Now - DateOfBirth).TotalDays / 365.25);
		}


		public int HourlyRate { get; set; }		
		public int HoursWorked { get; set; }    // recalulated on the last day of the month

		public Employee(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, 
						string positionTitle = "Employee", 
						Positions position = Positions.Employee,
						int hourlyRate = 12)
				: base(FN, LN, DOB, hired, deptID, positionTitle, position)
		{
			this.HourlyRate = hourlyRate;
			this.HoursWorked = 22 * 8;
		}

		// for an employee salaryBase is an hourly base of payment
		public override int Salary
		{
			get { return HourlyRate * HoursWorked; }
			set {  } // do nothing
		}

		/// <summary>
		/// Constructor to change type of worker to Employee
		/// ID and common properties will be copied to new instance of the Employee class 
		/// position related, HourlyRate and HoursWorked properties need to be updated additionally
		/// after creating new instance of the Employee
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Employee(IWorkerDTO w)
			: base (w.ID)
		{
			this.FirstName		= w.FirstName;
			this.LastName		= w.LastName;
			this.DateOfBirth	= w.DateOfBirth;
			this.EmployedOn		= w.EmployedOn;
			this.DeptID			= w.DeptID;
		}


		public Employee(Worker w)
			: base(w.ID)
		{
			this.FirstName	 = w.FirstName;
			this.LastName	 = w.LastName;
			this.DateOfBirth = w.DateOfBirth;
			this.EmployedOn	 = w.EmployedOn;
			this.DeptID		 = w.DeptID;
		}

		public static explicit operator Employee(Director w)
		{
			return new Employee(w)
			{
				Position		= Positions.Employee,
				PositionTitle	= "Employee",
				HourlyRate		= 12,
				HoursWorked		= 22 * 8
			};
		}
	}
}
