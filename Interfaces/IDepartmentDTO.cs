using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	public interface IDepartmentDTO
	{
		/// <summary>
		/// Unique department ID
		/// </summary>
		uint DeptID { get; }

		/// <summary>
		/// Department name
		/// </summary>
		string DeptName { get; set; }

		/// <summary>
		/// Department creation date
		/// </summary>
		DateTime CreatedOn { get; set; }

		int NumberOfSubDepts { get; }

		/// <summary>
		/// Number of department employees
		/// I'm not sure if we need this field. It will require addition logic to update it
		/// But may be good to keep for consistency purposes
		/// </summary>
		int NumberOfEmployees { get; set; }

		/// <summary>
		/// Sum of salaries received by
		/// - all dept workers - employees & interns
		/// - all sub-departments
		/// - director. 
		/// Director's salary is 15% of salaries of all dept workers and all sub-departments
		/// /// </summary>
		int TotalDepartmentSalary { get; set; }

		// Sum of salaries of all department staff except director
		int TotalDeptStaff_withoutBoss_Salary { get; set; }

		// Sum of Total Department salaries of all sub departments
		int TotalSubDepartmentsSalary { get; set; }
	}
}
