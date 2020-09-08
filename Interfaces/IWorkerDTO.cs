using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	public interface IWorkerDTO
	{
		/// <summary>
		/// Unique worker's Id
		/// </summary>
		uint ID { get; }

		/// <summary>
		/// Worker's first name
		/// </summary>
		string FirstName { get; set; }

		/// <summary>
		/// Worker's last name
		/// </summary>
		string LastName { get; set; }

		/// <summary>
		/// Worker's date of birth
		/// </summary>
		DateTime DateOfBirth { get; set; }

		/// <summary>
		/// Date of hiring in organization
		/// </summary>
		DateTime EmployedOn { get; set; }

		/// <summary>
		/// ID of a department worker works in
		/// </summary>
		uint DeptID { get; set; }

		/// <summary>
		/// Current text of position.
		/// Amended by EditWorker method based on Position property value and 
		/// department of worker
		/// </summary>
		string PositionTitle { get; set; }

		/// <summary>
		/// Position - enum type of several possible posisitons
		/// withing organization President, VP, Head of Division, Vice Head, Dept Director, Vice Dir, Employee, Intern
		/// </summary>
		Positions Position { get; set; }       // 

		/// <summary>
		/// Base how salary is calculated: money. For Employee type - hours worked
		/// </summary>
		int salaryBase { get; set; }

		/// <summary>
		/// Worker's salary calculated based on salaryBase or other parameters
		/// </summary>
		int Salary { get; set; }

		object Clone();
	}
}
