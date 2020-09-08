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
	}
}
