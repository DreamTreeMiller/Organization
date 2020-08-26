using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class DepartmentsTable
	{
		/// <summary>
		/// List (database table) of organization departments
		/// </summary>
		private List<BaseDepartment> Departments { get; set; }

		/// <summary>
		/// Constructor. Initializes Departments collection
		/// </summary>
		public DepartmentsTable()
		{
			Departments = new List<BaseDepartment>();
		}

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
		public int AddDepartment(BaseDepartment parentDept, BaseDepartment childDept)
		{
			// Creation of root department
			// it means childDept is root department
			if (parentDept == null && childDept.ParentDept == 0)
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
			childDept.ParentDept = parentDept.DeptID;
			return 0;
		}

		/// <summary>
		/// Find department with specified ID
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		/// Department with specified ID
		/// null if a department with such ID does not exist
		/// </returns>
		public BaseDepartment GetDepartment(uint deptID)
		{
			int di = Departments.FindIndex(d => d.DeptID == deptID);
			if (di != -1)
				return Departments[di];
			return null;
		}

		/// <summary>
		/// Returns root department. Its parent's Dept ID is 0
		/// </summary>
		/// <returns>Root department</returns>
		public BaseDepartment GetRootDepartment()
		{
			int di = Departments.FindIndex(d => d.ParentDept == 0);
			if (di != -1)
				return Departments[di];
			return null;

		}

		/// <summary>
		/// Returns root department ID
		/// </summary>
		/// <returns>uint ID of root department</returns>
		public uint GetRootDeptID()
		{
			return GetRootDepartment().DeptID;
		}

		/// <summary>
		/// Returns a copy of list of departments
		/// </summary>
		/// <returns></returns>
		public List<BaseDepartment> GetDepartmentsList()
		{

			return new List<BaseDepartment>(Departments);
		}

		/// <summary>
		/// Collects list of sub-departments of the specified department
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		/// Collection of departments which have specified department ID as parent department ID
		/// </returns>
		public List<BaseDepartment> SubDepartments(uint deptID)
		{
			return Departments.FindAll(w => w.ParentDept == deptID); ;
		}

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
		public int RemoveEmptyDepartment(uint deptID)
		{
			throw new NotImplementedException();
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
