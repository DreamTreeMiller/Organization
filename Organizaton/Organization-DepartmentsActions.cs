using MLM.Interfaces;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Organizaton
{
	public partial class Organization : IDepartmentsActions
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
		public int AddDepartment(IDepartmentDTO pd, IDepartmentDTO cd)
		{
			BaseDepartment parentDept = pd as BaseDepartment;
			BaseDepartment childDept  = cd as BaseDepartment;

			// Creation of root department
			// it means childDept is root department
			if (parentDept == null && childDept.ParentDeptID == 0)
			{
				Departments.Add(childDept);
				return 0;
			}

			// Checking if parents and child are not the same
			if (parentDept.DeptID == childDept.DeptID) return -2;

			// Checking if parent does not contain child
			if (parentDept.SubDepts.Contains(childDept.DeptID)) return -1;

			// Adding child department to 
			Departments.Add(childDept);
			parentDept.SubDepts.Add(childDept.DeptID);
			childDept.ParentDeptID = parentDept.DeptID;
			return 0;
		}

		/// <summary>
		/// Removes department with specified ID from the list of departments
		/// provided that this is not root department.
		/// Adds sub departments to the parent department.
		/// Adds all department workers to the parent department.
		/// </summary>
		public int SelfExcludeOfDepartment(uint deptID)
		{
			//ParentDept.Employees.AddRange(this.Employees);
			//foreach (Department d in SubDepts) d.ParentDept = this.ParentDept;
			//ParentDept.SubDepts.AddRange(this.SubDepts);
			return 0;
		}
	}
}
