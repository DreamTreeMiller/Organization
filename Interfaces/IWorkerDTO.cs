using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	/// <summary>
	/// Interface to transer new worker's data
	/// from UI part of Edit worker to 
	/// back end of edit worker
	/// </summary>
	public interface IWorkerDTO : IWorker
	{
		WorkerHierarchy OriginalWorkerType { get; set; }
		/// <summary>
		/// New type of worker
		/// </summary>
		WorkerHierarchy SelectedWorkerType { get; set; }

		//uint ID { get; }
		//string FirstName { get; set; }
		//string LastName { get; set; }

		//DateTime DateOfBirth { get; set; }
		//DateTime EmployedOn { get; set; }
		//uint DeptID { get; set; }
		//Positions Position { get; set; }
		//string PositionTitle { get; set; }

		#region Salaries

		// Director class does not need salary field
		// becaus his salary is automatically calculated base on subordinates' salaries

		/// <summary>
		/// For calculation of Employee's salary - hourly rate
		/// </summary>
		int HourlyRate { get; set; }

		/// <summary>
		/// For calculation of Employee's salary - hours worked
		/// </summary>
		int HoursWorked { get; set; }

		/// <summary>
		/// New salary of intern
		/// </summary>
		int IntSalary { get; set; }

		#endregion
	}
}
