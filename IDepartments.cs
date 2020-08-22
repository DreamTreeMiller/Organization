using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	interface IDepartments
	{
		/// <summary>
		/// Finds department with specified ID
		/// </summary>
		/// <param name="deptID"></param>
		/// <returns>Department with specified ID or null if such dept does not exist</returns>
		Department GetDepartment(uint deptID);

		/// <summary>
		/// Returns root department. Its parent's Dept ID is 0
		/// </summary>
		/// <returns>Root department</returns>
		Department GetRootDepartment();

		/// <summary>
		/// Returns root department ID
		/// </summary>
		/// <returns>uint ID of root department</returns>
		uint GetRootDeptID();

		/// <summary>
		/// Collects list of sub-departments of the specified department
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		/// Collection of departments which have specified department ID as parent department ID
		/// </returns>
		List<Department> SubDepartments(uint deptID);

		/// <summary>
		/// Adds child department to parent department
		/// </summary>
		/// <param name="parentDept">Parent department</param>
		/// <param name="childDept">Departmet to add</param>
		/// <returns>
		///  0 if child was added successfully, 
		/// -1 if child already exists, 
		/// -2 if parent and child are the same
		/// </returns>
		int AddDepartment(Department parentDept, Department childDept);

		/// <summary>
		/// Updates department with the same ID as 
		/// </summary>
		/// <param name="deptID">ID of department to update</param>
		/// <param name="dept">Content </param>
		/// <returns></returns>
		//int UpdateDepartment(uint deptID, Department dept);


		/// <summary>
		/// Removes department with specified ID from the list of departments
		/// provided that 
		/// (1) this is not root department 
		/// (2) the department does not have any workers and subdepartments
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		///  0 - department was removed successfully
		/// -1 - department is not empty, either workers or sub departments exist
		/// -2 - this is root department
		/// -3 - department with such ID does not exist
		/// </returns>
		int RemoveEmptyDepartment(uint deptID);

		/// <summary>
		/// Removes department with specified ID from the list of departments
		/// provided that this is not root department.
		/// Adds sub departments to the parent department.
		/// Adds all department workers to the parent department.
		/// </summary>
		int SelfExcludeOfDepartment(uint deptID);
	}
}
