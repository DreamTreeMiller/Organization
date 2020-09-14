using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.InterfacesActions
{
	public interface IDepartmentsActions
	{
		/// <summary>
		/// Adds newly created child department to parent department
		/// </summary>
		/// <param name="parentDept">Parent department</param>
		/// <param name="childDept">Departmet to add</param>
		/// <returns>
		///  0 if child was added successfully, 
		/// -1 if paretn already has child dept with the same ID, 
		/// -2 if parent and child are the same
		/// </returns>
		int AddDepartment(IDepartmentDTO parentDept, IDepartmentDTO childDept);

		/// <summary>
		/// Adds newly created child department to parent department
		/// </summary>
		/// <param name="pd">Parent department</param>
		/// <param name="childDeptName">Name of the department to be created</param>
		/// <param name="childDeptCreationDate">Child department creation date</param>
		/// <returns>
		/// </returns>
		IDepartmentDTO CreateDepartment(IDepartmentDTO pd, string childDeptName, DateTime childDeptCreationDate);

		/// <summary>
		/// Updates name and creation date of the department
		/// </summary>
		/// <param name="deptToEdit">Department to edit</param>
		/// <param name="newName">New department name</param>
		/// <param name="newCreationDate">New creation date</param>
		/// <returns>
		/// 0 if department was updated successfully
		/// </returns>
		int EditDepartment(IDepartmentDTO deptToEdit, string newName, DateTime newCreationDate);

		/// <summary>
		/// Returns a list of departments where we can move current department because
		/// department cannot be moved to any its direct or distant subdepartments 
		/// </summary>
		/// <param name="d">Department to be moved</param>
		/// <returns>List of available departments to move specified one</returns>
		List<IDepartmentDTO> AvailableDestinationDepartmentsList(IDepartmentDTO d);

		/// <summary>
		/// Moves department to destination department
		/// </summary>
		/// <param name="d">Department to move</param>
		/// <param name="destinationDept">Destination department to nest in as subdepartment</param>
		/// <returns>Moved department with updated parent dept ID</returns>
		IDepartmentDTO MoveDepartment(IDepartmentDTO d, IDepartmentDTO destinationDept);


		/// <summary>
		/// Deletes department which has no employees and sub departments
		/// </summary>
		/// <param name="d">Department to delete</param>
		/// <returns>Deleted department</returns>
		IDepartmentDTO DeleteEmptyDepartment(IDepartmentDTO d);

		/// <summary>
		/// Removes department with specified ID from the list of departments
		/// provided that this is not root department.
		/// Adds sub departments to the parent department.
		/// Adds all department workers to the parent department.
		/// </summary>
		/// <param name="d">Department to delete</param>
		/// <returns>
		/// Deleted department
		/// null if such department does not exist or we are trying to delete root department
		/// </returns>
		IDepartmentDTO SelfExcludeOfDepartment(IDepartmentDTO d);

		/// <summary>
		/// Deletes department including all sub departments with all employees
		/// </summary>
		/// <param name="d">Department to delete</param>
		void DeleteCompletely(IDepartmentDTO d);
	}
}
