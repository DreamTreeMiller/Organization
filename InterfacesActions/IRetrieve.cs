using MLM.Interfaces;
using System.Collections.Generic;

namespace MLM.InterfacesActions
{
	public interface IRetrieve
	{
		/// <summary>
		/// Returns root department of organization
		/// </summary>
		/// <returns>Root department</returns>
		IDepartmentDTO RootDepartment();

		/// <summary>
		/// Finds director (president,head) of department
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>
		/// Worker of the Director class or 
		/// null if a director of deptID department is not found
		/// </returns>
		IWorker Director(IDepartmentDTO d);

		/// <summary>
		/// Finds all workers, including director, who work in the department d
		/// and puts them in the collection List
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>Collection of workers of IWorker type who works in the department d</returns>
		List<IWorker> OneDepartmentWorkersList(IDepartmentDTO d);

		/// <summary>
		/// Returns a list of all organization departments
		/// </summary>
		/// <returns></returns>
		List<IDepartmentDTO> DepartmentsList();

		/// <summary>
		/// Collects list of sub-departments of the specified department
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>
		/// Collection of sub departments of department d, or 
		/// null if there are no sub departments
		/// </returns>
		List<IDepartmentDTO> SubDepartments(IDepartmentDTO d);
	}
}
