using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Department : IManageEmployees
	{
		public ulong DepID					{ get; }
		public string Name					{ get; set; }
		public DateTime CreatedOn			{ get; set; }
		public Department ParentDept		{ get; set; }
		//public Director Director			{ get; set; }
		public List<Worker> Employees		{ get; set; }
		public uint TotalDepartmentSalary	{ get; set; }		// Sum of salaries received by
																// - all dept workers - employees & interns
																// - all sub-departments
																// - director
		public List<Department> SubDepts	{ get; set; }

		public Department(string dptName, Department parentDpt)
		{
			// We take first 6 bytes of Guid and compose a unique ulong ID out of them
			byte[] guid = Guid.NewGuid().ToByteArray();
			DepID = ((ulong)guid[3] << 8 * 5) |        // This is the oder of bytes 
					((ulong)guid[2] << 8 * 4) |        // according to the string format of Guid
					((ulong)guid[1] << 8 * 3) |
					((ulong)guid[0] << 8 * 2) |
					((ulong)guid[5] << 8 * 1) |
					 (ulong)guid[4];
			Name					= dptName;
			CreatedOn				= DateTime.Now;
			ParentDept				= parentDpt;
			Employees				= new List<Worker>();
			TotalDepartmentSalary	= 0;
			SubDepts				= new List<Department>();
		}
		public Director GetDirector()
		{
			foreach (Worker w in Employees)
				if (w is Director) return w as Director;
			return null;
		}

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

		public int UpdateEmployee(Worker worker)
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

		public uint CalculateTotalDeptSalary(PaymentType pymnt)
		{
			TotalDepartmentSalary = 0;
			Random r = new Random();
			foreach (Worker w in Employees)
				if (!(w is Director))
				{
					w.GetPaid( (pymnt == PaymentType.Standard) ? 
														22 * 8 : 	// Standard 8 hours per 22 working days
										(uint)r.Next(100, 240));    // Random selection of hours
					TotalDepartmentSalary += w.Salary;
				}
			foreach (Department d in SubDepts)
				TotalDepartmentSalary += d.CalculateTotalDeptSalary(pymnt);

			Director dir = GetDirector();
			dir.GetPaid(TotalDepartmentSalary * 15 / 100);
			UpdateEmployee(dir);
			TotalDepartmentSalary += dir.Salary;
			return TotalDepartmentSalary;
		}

		public int AddDepartment(Department dpt)
		{
			foreach (Department d in SubDepts)
				if (d.DepID == dpt.DepID) return -1;        // Such department already exists
			SubDepts.Add(dpt);
			return 0;
		}

		public Department RemoveDepartment(Department dpt)
		{
			foreach (Department d in SubDepts)
				if (d.DepID == dpt.DepID)
				{
					SubDepts.Remove(d);
					return d;
				}
			return null;
		}

		public Department RemoveDepartment(uint dptID)
		{
			foreach (Department d in SubDepts)
				if (d.DepID == dptID)
				{
					SubDepts.Remove(d);
					return d;
				}
			return null;
		}

		public void SelfExclude()
		{

		}
	
	
	}
}
