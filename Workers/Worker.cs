using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MLM
{
	public abstract class Worker
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
		
		/// <summary>
		/// Current text of position.
		/// Amended by EditWorker method based on Position property value and 
		/// department of worker
		/// </summary>
		public string	PositionTitle { get; set; }

		/// <summary>
		/// Position - enum type of several possible posisitons
		/// withing organization President, VP, Head of Division, Vice Head, Dept Director, Vice Dir, Employee, Intern
		/// </summary>
		public Positions Position	{ get; set; }       // 

		/// <summary>
		/// Worker's salary calculated based on worker type
		/// </summary>
		public abstract int Salary { get; set; }

		/// <summary>
		/// Constructor to create dummy worker with particular ID 
		/// in order to change class of a worker! 
		/// Because type casting will not work in this case:
		/// Employee emp; Director dir; dir = emp as Director; will assign null to dir.
		/// </summary>
		/// <param name="workerID">Worker's ID</param>
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
		public Worker(string FN, string LN, DateTime DOB, 
						DateTime hired,
						uint depID, string positionTitle, Positions position)
		{
			this.ID				= NextID(); 
			this.FirstName		= FN;
			this.LastName		= LN;
			this.DateOfBirth	= DOB;
			this.EmployedOn		= hired;
			this.DeptID			= depID;
			this.PositionTitle	= positionTitle;
			this.Position		= position;
		}

		public override string ToString()
		{
			return	$"ID: {ID,16} " +
					$"Name: {FirstName, 12} {LastName,12} " +
					$"DOB: {DateOfBirth:dd.MM.yyyy} " +
					$"Hired: {EmployedOn:dd.MM.yyyy} " +
					$"DepID: {DeptID,5} " +
					$"{PositionTitle,30} " + 
					$"Salary: ${Salary:## ###}";
		}
	}
}
