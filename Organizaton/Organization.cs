using MLM.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MLM.Organizaton
{
	public partial class Organization : IOrganization 
	{
		/// <summary>
		/// List (database table) of organization departments
		/// </summary>
		public List<BaseDepartment> Departments { get; set; }

		/// <summary>
		/// Collection (DataBase table) of all workers of the organization
		/// </summary>
		public List<Worker> Workers { get; set; }

		/// <summary>
		/// Keeps list of all positions in the organization.
		/// Provides list of positions available in different departments and situations
		/// </summary>
		private PositionsTable _positions { get; }

		/// <summary>
		/// Constructor. Creates root department with 0 as parent dept ID, 
		/// and with 0 workers and 0 subdepartments. Adds it to the collection of all departments
		/// </summary>
		/// <param name="orgName">Name of the organization. It is assigned to the root dept name.</param>
		public Organization()
		{
			Departments = new List<BaseDepartment>();
			Workers = new List<Worker>();
			_positions = new PositionsTable(this);
		}

		#region IOrganization interface implementation

		/// <summary>
		/// List of positions in the company. 
		/// It is necessary to access to positions via indexer.
		/// IPositions provides indexer access to positions
		/// </summary>
		public IPositions PositionsData { get => _positions; }

		/// <summary>
		/// Returns positions available for specified department
		/// </summary>
		/// <param name="dept">Department</param>
		/// <returns></returns>
		public List<IPositionTuple> Available(IDepartmentDTO dept, bool keepDirector)
		{
			return _positions.Available(dept as BaseDepartment, keepDirector);
		}

		#endregion

		#region Salary manipulations

		/// <summary>
		/// Updates salaries of current department and all upper departments.
		/// Not so efficient, but simple. If there is no time constraints, it's Ok
		/// </summary>
		/// <param name="dept">Department which workers salaries must be updated, and all above</param>
		public void UpdateSalaries(BaseDepartment dept)
		{
			// Collect salaries of all sub departments
			dept.TotalSubDepartmentsSalary =
				dept.SubDepts.Sum(d =>
				{
					var sd = Department(d);
					int sdSalary = 0;
					if (sd != null) sdSalary = sd.TotalDepartmentSalary;
					return sdSalary;
				});

			// Calculate total salay of all staff except director
			var dir = Director(dept);
			int dirSalary = 0;
			dept.TotalDeptStaff_withoutBoss_Salary =
				OneDepartmentWorkersList(dept).Sum(w => w.Salary);

			// Update director's salary
			if (dir != null)
			{
				dept.TotalDeptStaff_withoutBoss_Salary -= dir.Salary;
				dirSalary = (dept.TotalDeptStaff_withoutBoss_Salary +
							 dept.TotalSubDepartmentsSalary)
							 / 100 * 15;
				dir.Salary = dirSalary;
			}

			dept.TotalDepartmentSalary =
				dirSalary +
				dept.TotalDeptStaff_withoutBoss_Salary +
				dept.TotalSubDepartmentsSalary;

			// Check if we reached root department
			if (dept.ParentDeptID == 0) return;

			// Go up
			UpdateSalaries(Department(dept.ParentDeptID) as BaseDepartment);
		}
		#endregion

	}
}
