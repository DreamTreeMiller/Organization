using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.InterfacesActions
{
	public interface IWorkersActions
	{
		/// <summary>
		/// Creates new worker of specified type and adds him to the organization
		/// </summary>
		/// <param name="fn">First name</param>
		/// <param name="ln">Last name</param>
		/// <param name="dob">Date of birth</param>
		/// <param name="d">Department interface</param>
		/// <param name="pos">Position</param>
		/// <returns>
		///  0 if worker was added successfully, 
		/// -1 if worker with same ID already exists which should not happen in principle
		/// because worker is newly created,
		/// -2 if worker is director and department already has a director
		/// </returns>
		int AddWorker(string firstName, string lastName, DateTime dateOfBirth, 
				IDepartmentDTO d, Positions pos);

		/// <summary>
		/// Updates worker with new data from
		/// </summary>
		/// <param name="">Updated worker</param>
		/// <returns>
		///  0 if worker was updated successfully
		/// -1 if worker with such ID was not found
		/// </returns>
		int EditWorker(IWorkerDTO updatedW);

		/// <summary>
		/// Moves worker to the department with specified ID
		/// </summary>
		/// <param name="worker">Worker to move</param>
		/// <param name="destDept">Destination department</param>
		/// <returns>
		/// 0 if moved successfully
		/// -1 if worker with such ID does not exist
		/// -2 if department with such ID does not exist
		/// -3 if destination department is current worker's department
		/// </returns>
		int MoveWorker(IWorker worker, IDepartmentDTO destDept);

		/// <summary>
		/// Completely removes worker with specified ID from the list
		/// </summary>
		/// <param name="w">Worker to remove</param>
		/// <returns>
		/// Removed worker, if he was removed successfully, 
		/// null if the worker was not found
		/// </returns>
		IWorker RemoveWorker(IWorker w);
	}
}
