using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Department : IManageEmployees
	{
		public uint DepID { get; }
		public string DepName		{ get; set; }
		public DateTime CreatedOn	{ get; set; }
		public Department ParentDept { get; set; }
		public Director Director	{ get; set; }
		public List<Department> SubDepts { get; set; }
		public uint DepTotalSalary { get; set; }
		public List<Worker> Employees { get; set; }


		/// <summary>
		/// Finds in the department an employee with specified ID
		/// </summary>
		/// <param name="empID"></param>
		/// <returns>Found employee
		/// or null - if such employee does not exist in the department</returns>
		public Worker GetEmployee(ulong empID)
		{
			foreach (Worker w in Employees)
				if (w.ID == empID) return w;
			return null;
		}

		public int AddEmployee(Worker worker)
		{
			foreach (Worker w in Employees)
				if (w.ID == worker.ID) return -1;		// Worker with such ID already works in dept
			Employees.Add(worker);
			return 0;
		}

		public int EditEmployee(Worker worker)
		{
			if (RemoveEmployee(worker.ID) != null)
			{
				AddEmployee(worker);
				return 0;			// Employee was updated successfully
			}
			return -1;		// No such employee in the dept
		}

		public Worker RemoveEmployee(Worker worker)
		{
			if (Employees.Remove(worker)) return worker;
			else return null;
		}

		/// <summary>
		/// Completely removes employee with specified ID from the list
		/// </summary>
		/// <param name="empID">Employee ID</param>
		/// <returns>0 - employee was removed successfully
		/// -1 - employee was not found</returns>
		public Worker RemoveEmployee(ulong empID)
		{
			foreach (Worker w in Employees)
				if (w.ID == empID)
				{
					Employees.Remove(w);
					return w;
				}
			return null;	// such employee was not found
		}

		void CalculateEmployeesSalaries()
		{
			DepTotalSalary = 0;
			foreach (Worker w in Employees)
				if (w.Position != Positions.DivisionHead ||
					w.Position != Positions.DeptDirector)
					DepTotalSalary += w.Salary;
		}
	}
}
