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
			_positions = new PositionsTable();
		}

		#region IOrganization interface implementation

		/// <summary>
		/// List of positions in the company. 
		/// Method 'Available' returns positions available for specified department
		/// </summary>
		public IPositions PositionsData { get => _positions; }

		public List<IPositionTuple> Available(IDepartmentDTO dept)
		{
			return _positions.Available(this, dept as BaseDepartment);
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
