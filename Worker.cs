using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MLM
{
	abstract class Worker
	{
		/// <summary>
		/// Current ID to assign to the next worker
		/// </summary>
		private static uint staticID;

		/// <summary>
		/// Static constructor
		/// 
		/// </summary>
		static Worker()
		{
			staticID = 0;
		}

		/// <summary>
		/// Generates next available ID
		/// </summary>
		/// <returns>New unique ID</returns>
		private static uint NextID()
		{
			staticID++;
			return staticID;
		}

		/// <summary>
		/// Unique worker's Id
		/// </summary>
		public uint		ID			{ get;}				

		/// <summary>
		/// Worker's first name
		/// </summary>
		public string	FirstName	{ get; set; }

		/// <summary>
		/// Worker's last name
		/// </summary>
		public string	LastName	{ get; set; }

		/// <summary>
		/// Worker's date of birth
		/// </summary>
		public DateTime	DateOfBirth { get; set; }

		/// <summary>
		/// Date of hiring in organization
		/// </summary>
		public DateTime EmployedOn	{ get; set; }

		/// <summary>
		/// ID of a department worker works in
		/// </summary>
		public uint DeptID			{ get; set; }
		
		//public string DepName							// Name of department'
		// need to think how to implement

		/// <summary>
		/// For now this is just string format of Position 
		/// Not good decision yet ...
		/// </summary>
		public string	JobTitle	{ get; set; }

		/// <summary>
		/// Position - enum type of several possible posisitons
		/// withing organization President, VP, Head of Division, Vice Head, Dept Director, Vice Dir, Employee, Intern
		/// </summary>
		public Positions Position	{ get; set; }       // 
		
		/// <summary>
		/// Base how salary is calculated: money. For Employee type - hours worked
		/// </summary>
		public uint salaryBase		{ get; set; }

		/// <summary>
		/// Worker's salary calculated based on salaryBase or other parameters
		/// </summary>
		public abstract uint Salary { get; set; }       

		/// <summary>
		/// Constructor to create dummy worker with particular ID in order to check 
		/// if a worker with such ID already exists
		/// </summary>
		/// <param name="workerID"></param>
		public Worker(uint workerID)
		{
			this.ID = workerID;
		}

		/// <summary>
		/// Creates an instance of a worker
		/// </summary>
		/// <param name="FN">First name</param>
		/// <param name="LN">Last name</param>
		/// <param name="DOB">Date of birth</param>
		/// <param name="hired">Date of hiring in organization</param>
		/// <param name="depID">Department worker works in</param>
		/// <param name="jobTitle">String expression of Position</param>
		/// <param name="position">Position</param>
		/// <param name="salaryBase">Base to calculate salary</param>
		public Worker(string FN, string LN, DateTime DOB, 
						DateTime hired,
						uint depID, string jobTitle, Positions position,
						uint salaryBase = 0)
		{
			this.ID				= NextID(); 
			this.FirstName		= FN;
			this.LastName		= LN;
			this.DateOfBirth	= DOB;
			this.EmployedOn		= hired;
			this.DeptID			= depID;
			this.JobTitle		= jobTitle;
			this.Position		= position;
			this.salaryBase		= salaryBase;
		}

		/// <summary>
		/// Overriding of a standard Equals method. Two workers are equal if their IDs are the same.
		/// I am not sure yet, if this criteria is good ...
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// This line is necessary because DisconnectedItem exception is thrown
			// when we change department in TreeView
			// But I don't know why DataGrid calls Equals method ...
			if (obj == BindingOperations.DisconnectedSource) return false;
			Worker w = obj as Worker;
			return this.ID.Equals(w.ID);
		}

		/// <summary>
		/// Have to implement because Equals is implemented. 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return	$"ID: {ID,16} " +
					$"Name: {FirstName, 12} {LastName,12} " +
					$"DOB: {DateOfBirth:dd.MM.yyyy} " +
					$"Hired: {EmployedOn:dd.MM.yyyy} " +
					$"DepID: {DeptID,5} " +
					$"{JobTitle,30} " + 
					$"Salary: ${Salary:## ###}";
		}
	}
}
