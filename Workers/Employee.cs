using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Employee : Worker, IWorkerDTO
	{
		public int HoursWorked { get; set; }    // recalulated on the last day of the month

		public Employee(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, 
						string positionTitle = "Employee", 
						Positions position = Positions.Employee,
						int hourlyRate = 12)
				: base(FN, LN, DOB, hired, deptID, positionTitle, position, hourlyRate)
		{
			this.HoursWorked = 22 * 8;
		}

		// for an employee salaryBase is an hourly base of payment
		public override int Salary
		{
			get { return base.salaryBase * this.HoursWorked; }
			set { this.HoursWorked = value; }
		}

		/// <summary>
		/// Constructor to change type of worker to Employee
		/// ID and common fields will be copied to new instance of Employee class 
		/// Salary fields will be initialized as if new Employee was created
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Employee(Worker w)
			: base (w.ID)
		{
			this.FirstName		= w.FirstName;
			this.LastName		= w.LastName;
			this.DateOfBirth	= w.DateOfBirth;
			this.EmployedOn		= w.EmployedOn;
			this.DeptID			= w.DeptID;
			this.PositionTitle	= "Employee";
			this.Position		= Positions.Employee; ;
			this.salaryBase		= 12;
			this.HoursWorked	= 22 * 8;
		}
		public object Clone()
		{
			return (Employee)this.MemberwiseClone();
		}
	}
}
