﻿using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class Intern : Worker, IWorkerDTO
	{
		public override int Salary
		{
			get { return salaryBase;  }
			set { salaryBase = value; }		// Do nothing becase salary is already set by the constructor
		}
		public Intern (string FN, string LN, DateTime DOB,
					   DateTime hired,
					   uint deptID, string jobTitle, int salaryBase=500) 
				: base (FN, LN, DOB, hired, deptID, jobTitle, Positions.Intern, salaryBase)
		{
		}

		/// <summary>
		/// Constructor to change type of worker to Intern
		/// ID and common fields will be copied to new instance of Employee class 
		/// Salary fields will be initialized as if new Employee was created
		/// </summary>
		/// <param name="w">Worker to change to Employee class</param>
		public Intern(Worker w)
			: base(w.ID)
		{
			this.FirstName		= w.FirstName;
			this.LastName		= w.LastName;
			this.DateOfBirth	= w.DateOfBirth;
			this.EmployedOn		= w.EmployedOn;
			this.DeptID			= w.DeptID;
			this.PositionTitle	= "Intern";
			this.Position		= Positions.Intern;
			this.salaryBase		= 500;
		}

		public object Clone()
		{
			return (Intern)this.MemberwiseClone();
		}
	}
}
