using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	interface IManageEmployees
	{
		/// <summary>
		/// Returns employee with specified ID
		/// </summary>
		/// <param name="empID">Employee's ID</param>
		/// <returns>Employee, null if such employee does not work in the dept</returns>
		Worker GetEmployee(ulong empID);

		/// <summary>
		/// Adds an employee to the dept
		/// </summary>
		/// <param name="employee"></param>
		/// <returns>0 if an employee was added succesfully
		/// -1 - if an employee with same ID already works in the dept</returns>
		int AddEmployee(Worker worker);

		/// <summary>
		/// Removes specified employee from department
		/// </summary>
		/// <param name="worker"></param>
		/// <returns>Removed employee, null if such employee does not work in the dept</returns>
		Worker RemoveEmployee(Worker worker);

		/// <summary>
		/// Removes an employee with the specified ID from department
		/// </summary>
		/// <param name="worker"></param>
		/// <returns>Removed employee, 'null' if employee  with the specified ID does not work in the dept</returns>
		Worker RemoveEmployee(ulong empID);

		/// <summary>
		/// Updates data of an employee 
		/// </summary>
		/// <param name="worker"></param>
		/// <returns>0 if an employee was updated successfully, -1 if such employee does not work in the dept</returns>
		int EditEmployee(Worker worker);
	}
}
