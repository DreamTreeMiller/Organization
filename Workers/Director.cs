using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Director : Worker, IWorker, IDirector
	{
		private int _salary;

		public Director(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, string positionTitle, Positions position = Positions.DeptDirector,
						int salary = 1300)
				: base(FN, LN, DOB, hired, deptID, positionTitle, position)
		{
			_salary = salary;
		}

		public override int Salary
		{
			get { return _salary; }
			set { _salary = value > 1300 ? value : 1300; }
		}

		/// <summary>
		/// Constructor to change type of worker to Director
		/// ID and common fields will be copied to new instance of Director class 
		/// position related and Salary properties need to be updated additionally
		/// after creating new instance of the Director
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Director(IWorkerDTO w)
			: base(w.ID)
		{
			this.FirstName   = w.FirstName;
			this.LastName	 = w.LastName;
			this.DateOfBirth = w.DateOfBirth;
			this.EmployedOn	 = w.EmployedOn;
			this.DeptID		 = w.DeptID;
		}

		public Director(Worker w)
			: base(w.ID)
		{
			this.FirstName = w.FirstName;
			this.LastName = w.LastName;
			this.DateOfBirth = w.DateOfBirth;
			this.EmployedOn = w.EmployedOn;
			this.DeptID = w.DeptID;
			this.Salary = 1300;
		}

		//public object Clone()
		//{
		//	return (Director) this.MemberwiseClone();
		//}
	}
}
