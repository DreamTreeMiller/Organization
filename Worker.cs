using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public Department Department { get; set; }
		public string DepName => Department.Name;		// Name of department
		public string	JobTitle	{ get; set; }		// responsibilities
		public Positions Position	{ get; set; }       // withing organization President, VP, Head of Division, Dept Director
		public uint salaryBase { get; set; }            // Salary base. for each type of worker meaning is different
		public abstract uint Salary { get; set; }       // Salary - for each type calculated differently

		static ObservableCollection<Worker> AllEmployees { get; set; }
		public Worker(string FN, string LN, DateTime DOB, 
						DateTime hired,
						Department department, string jobTitle, Positions position,
						uint salaryBase = 0)
		{
			this.ID				= UniqueID.Generate(); 
			this.FirstName		= FN;
			this.LastName		= LN;
			this.DateOfBirth	= DOB;
			this.EmployedOn		= hired;
			this.Department		= department;
			this.JobTitle		= jobTitle;
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

		public override string ToString()
		{
			return	$"ID: {ID,16} " +
					$"Name: {FirstName, 12} {LastName,12} " +
					$"DOB: {DateOfBirth:dd.MM.yyyy} " +
					$"Hired: {EmployedOn:dd.MM.yyyy} " +
					$"{DepName,20} " +
					$"{JobTitle,30} " + 
					$"Salary: ${Salary:## ###}";
		}
	}
}
