using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Organization
	{
		private Random r = new Random();

		/// <summary>
		/// Collection of all departments of the organization
		/// </summary>
		public DepartmentsTable	DepartmentsTable { get; set; }

		/// <summary>
		/// Collection of all workers of the organization 
		/// </summary>
		public WorkersTable		WorkersTable	 { get; set; }

		/// <summary>
		/// Constructor. Creates root department with 0 as parent dept ID, 
		/// and with 0 workers and 0 subdepartments. Adds it to the collection of all departments
		/// </summary>
		/// <param name="orgName">Name of the organization. It is assigned to the root dept name.</param>
		public Organization (string orgName) 
		{
			Random r = new Random();
			Department root = new Department(orgName, 0);
			root.CreatedOn = new DateTime(r.Next(2000, 2020), r.Next(1, 13), r.Next(1, 29));
			DepartmentsTable = new DepartmentsTable();
			WorkersTable = new WorkersTable();
			AddDepartment(null, root);
		}

		/// <summary>
		/// Moves worker to the department with specified ID
		/// </summary>
		/// <param name="workerID">Worker's ID</param>
		/// <param name="destinationDeptID">ID of destination department</param>
		/// <returns>
		/// 0 if moved successfully
		/// -1 if worker with such ID does not exist
		/// -2 if department with such ID does not exist
		/// </returns>
		public int MoveWorker(uint workerID,  uint destinationDeptID)
		{
			Worker w = WorkersTable.GetWorker(workerID);
			if (w == null) return -1;
			if (GetDepartment(destinationDeptID) == null) return -2;
			w.DeptID = destinationDeptID;
			return 0;
		}

		#region Salary manipulations

		/// <summary>
		/// Calculates total salary of specified department by calculating salaries of:
		/// - employees, inters
		/// - sub departments
		/// - director whose salary is calculated based on dept employees' and sub departments' salaries
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <param name="pymnt">Type of payment: 
		/// Standard - all employees work same number of hours.
		/// Random - each employee is assigned with a random number of hours between 100 and 240.
		/// </param>
		/// <returns>Total salary of the specified department</returns>
		public int CalculateTotalDeptSalary(uint deptID, PaymentType pymnt)
		{
			int TotalDeptSal = 0;
			foreach (Worker w in DepartmentWorkersList(deptID))
				if (!(w is Director))
				{
					w.Salary = (pymnt == PaymentType.Standard) ?
														22 * 8 :    // Standard 8 hours per 22 working days
											   r.Next(100, 240);    // Random selection of hours
					TotalDeptSal += w.Salary;
				}
			foreach (Department d in SubDepartments(deptID))
				TotalDeptSal += CalculateTotalDeptSalary(d.DeptID, pymnt);

			Director dir = GetDirector(deptID);
			dir.Salary = TotalDeptSal / 100 * 15;
			//UpdateEmployee(dir);
			TotalDeptSal += dir.Salary;
			GetDepartment(deptID).TotalDepartmentSalary = TotalDeptSal;
			return TotalDeptSal;
		}

		/// <summary>
		/// Updates total department salary of specified department and all parent departments up to the root
		/// </summary>
		/// <param name="startDeptID">ID of starting department</param>
		/// <param name="newTotalDeptSalary">Salary difference either positive or negative</param>
		private void UpdateUpperDeptSalaries(uint startDeptID, int salaryDiff)
		{
			Department d = DepartmentsTable.GetDepartment(startDeptID);
			Director dir = WorkersTable.GetDirector(startDeptID);

			// Important!!! This method is called when a worker is already updated, added or deleted)
			// but Total Department Salary is still NOT updated!
			int currTotalDeptSal = d.TotalDepartmentSalary;
			int newSubDeptAndWorkersSalary = currTotalDeptSal + salaryDiff;

			if (dir != null)
			{
				// ==> if director present - one way to update salary

				// Get new total workers and sub-dept salaries in order to calculate new salary of dept boss
				newSubDeptAndWorkersSalary -= dir.Salary;
				dir.Salary = newSubDeptAndWorkersSalary / 100 * 15;
				d.TotalDepartmentSalary = newSubDeptAndWorkersSalary + dir.Salary;
			}
			else
			{
				// ==> if not another - new total department salary is already calculated
				d.TotalDepartmentSalary = newSubDeptAndWorkersSalary;
			}
			if (d.ParentDept == 0) return;
			UpdateUpperDeptSalaries(d.ParentDept, d.TotalDepartmentSalary - currTotalDeptSal);
		}
		#endregion

		#region Implementation of IWorkers interface

		/// <summary>
		/// Adds worker to the organization workers' list. 
		/// Worker must have valid dept ID. 
		/// Number of workers of the specified department is increased by 1
		/// </summary>
		/// <param name="worker"></param>
		/// <returns>
		///  0 if worker was added successfully
		/// -1 if worker with same ID already exists
		/// </returns>
		public int AddWorker(Worker worker)
		{
			int result = WorkersTable.AddWorker(worker);
			if (result == 0)
				GetDepartment(worker.DeptID).NumberOfEmployees++;
			return result;
		}

		public Worker GetWorker(uint workerID)
		{
			return WorkersTable.GetWorker(workerID);
		}

		public Director GetDirector(uint deptID)
		{
			return WorkersTable.GetDirector(deptID);
		}

		public List<Worker> DepartmentWorkersList(uint deptID)
		{
			return WorkersTable.DepartmentWorkersList(deptID);
		}

		public Worker RemoveWorker(uint workerID)
		{
			Worker w = WorkersTable.RemoveWorker(workerID);
			if (w != null)
			{
				UpdateUpperDeptSalaries(w.DeptID, -w.Salary);
				GetDepartment(w.DeptID).NumberOfEmployees--;
			}
			return w;
		}

		#endregion

		#region Implementation of IDepartments interface

		public Department GetDepartment(uint deptID)
		{
			return DepartmentsTable.GetDepartment(deptID);
		}

		public Department GetRootDepartment()
		{
			return DepartmentsTable.GetRootDepartment();
		}

		public uint GetRootDeptID()
		{
			return DepartmentsTable.GetRootDeptID();
		}

		public List<Department> SubDepartments(uint deptID)
		{
			return DepartmentsTable.SubDepartments(deptID);
		}

		public int AddDepartment(Department parentDept, Department childDept)
		{
			return DepartmentsTable.AddDepartment(parentDept, childDept);
		}

		public int RemoveEmptyDepartment(uint deptID)
		{
			return DepartmentsTable.RemoveEmptyDepartment(deptID);
		}

		public int SelfExcludeOfDepartment(uint deptID)
		{
			return DepartmentsTable.SelfExcludeOfDepartment(deptID);
		}

		#endregion

	}
}
