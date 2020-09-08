using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Director : Worker, IWorkerDTO
	{
		public Director(string FN, string LN, DateTime DOB,
						DateTime hired,
						uint deptID, string positionTitle, Positions position = Positions.DeptDirector,
						int salaryBase = 1300)
				: base(FN, LN, DOB, hired, deptID, positionTitle, position, salaryBase)
		{ }

		public override int Salary
		{
			get { return base.salaryBase; }
			set { base.salaryBase = value > 1300 ? value : 1300; }
		}

		/// <summary>
		/// Constructor to change type of worker to Director
		/// ID and common fields will be copied to new instance of Director class 
		/// Salary fields will be initialized as if new Director was created
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Director(Worker w, Positions newPosition)
			: base(w.ID)
		{
			this.FirstName = w.FirstName;
			this.LastName = w.LastName;
			this.DateOfBirth = w.DateOfBirth;
			this.EmployedOn = w.EmployedOn;
			this.DeptID = w.DeptID;
			this.Position = newPosition;
			this.salaryBase = 1300;
		}

		public object Clone()
		{
			return (Director) this.MemberwiseClone();
		}
	}
}
