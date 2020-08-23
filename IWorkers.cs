using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	interface IWorkers
	{
		/// <summary>
		/// Adds new worker to the worker's list. Checks if a worker with the same ID already exists
		/// </summary>
		/// <param name="worker"></param>
		/// <returns></returns>
		int AddWorker(Worker worker);

		/// <summary>
		/// Finds woker with specified ID
		/// </summary>
		/// <param name="workerID"></param>
		/// <returns>Worker with specified ID 
		/// or null - if worker with such ID does not exist</returns>
		Worker GetWorker(uint workerID);

		/// <summary>
		/// Finds director (president,head) of deptID department
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>Worker of the Director class</returns>
		Director GetDirector(uint deptID);

		/// <summary>
		/// Finds all workers who work in the same department with deptID
		/// and puts them in the collection List
		/// </summary>
		/// <param name="deptID">ID of the department</param>
		/// <returns>Collection of workers who works in the department with deptID</returns>
		List<Worker> DepartmentWorkersList(uint deptID);

		/// <summary>
		/// Updates worker with the same ID
		/// </summary>
		/// <param name="worker"></param>
		/// <returns></returns>
		//int UpdateWorker(Worker worker);

		/// <summary>
		/// Completely removes worker with specified ID from the list
		/// </summary>
		/// <param name="workerID">Employee ID</param>
		/// <returns>
		/// Removed worker if he was removed successfully, 
		/// null if the worker was not found
		/// </returns>
		Worker RemoveWorker(uint workerID);
	}
}
