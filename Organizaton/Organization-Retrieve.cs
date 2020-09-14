using MLM.Interfaces;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Organizaton
{
	public partial class Organization : IRetrieve
	{
		#region Retrieving workers information interface implementation

		/// <summary>
		/// Finds director (president,head) of department
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>
		/// Worker of the Director class or 
		/// null if a director of deptID department is not found
		/// </returns>
		public IWorker Director(IDepartmentDTO d)
		{
			Director dir = Workers.Find(w => w.DeptID == d.DeptID && w is Director) as Director;
			return dir;
		}

		/// <summary>
		/// Finds all workers, including director, who work in the department d
		/// and puts them in the collection List
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>Collection of workers of IWorker type who works in the department d</returns>
		public List<IWorker> OneDepartmentWorkersList(IDepartmentDTO d)
		{
			List<IWorker> wl = new List<IWorker>();
			foreach (Worker w in Workers)
				if (w.DeptID == d.DeptID)
					wl.Add(w as IWorker);
			return wl;
		}

		#endregion

		#region Retrieving departments information interface implementation

		/// <summary>
		/// Returns root department of organization
		/// </summary>
		/// <returns>Root department</returns>
		public IDepartmentDTO RootDepartment()
		{
			BaseDepartment dept = Departments.Find(d => d.ParentDeptID == 0);
			return dept;
		}

		/// <summary>
		/// Find department with specified ID
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		/// Department with specified ID
		/// null if a department with such ID does not exist
		/// </returns>
		public IDepartmentDTO Department(uint deptID)
		{
			return Departments.Find(d => d.DeptID == deptID);
		}

		/// <summary>
		/// Returns a list of all organization departments
		/// </summary>
		/// <returns></returns>
		public List<IDepartmentDTO> DepartmentsList()
		{
			List<IDepartmentDTO> dl = new List<IDepartmentDTO>();
			foreach (var d in Departments)
				dl.Add(d as IDepartmentDTO);
			return dl;
		}

		/// <summary>
		/// Collects list of sub-departments of the specified department
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns>
		/// Collection of sub departments of department d, or 
		/// null if there are no sub departments
		/// </returns>
		public List<IDepartmentDTO> SubDepartments(IDepartmentDTO d)
		{
			List<IDepartmentDTO> dl = new List<IDepartmentDTO>();
			foreach (BaseDepartment dept in Departments)
				if (dept.ParentDeptID == d.DeptID)
					dl.Add(dept);
			return dl;
		}

		#endregion

	}
}
