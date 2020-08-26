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
		private DepartmentsTable	DepartmentsTable { get; set; }

		/// <summary>
		/// Collection of all workers of the organization 
		/// </summary>
		private WorkersTable		WorkersTable	 { get; set; }

		/// <summary>
		/// Constructor. Creates root department with 0 as parent dept ID, 
		/// and with 0 workers and 0 subdepartments. Adds it to the collection of all departments
		/// </summary>
		/// <param name="orgName">Name of the organization. It is assigned to the root dept name.</param>
		public Organization (string orgName) 
		{
			Random r = new Random();
			BaseDepartment root = new HQ(orgName);
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
			uint oldDeptID = w.DeptID;
			UpdateUpperDeptSalaries(oldDeptID, -w.Salary);
			w.DeptID = destinationDeptID;
			UpdateUpperDeptSalaries(destinationDeptID, w.Salary);
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
			foreach (Worker w in OneDepartmentWorkersList(deptID))
				if (!(w is Director))
				{
					w.Salary = (pymnt == PaymentType.Standard) ?
														22 * 8 :    // Standard 8 hours per 22 working days
											   r.Next(100, 240);    // Random selection of hours
					TotalDeptSal += w.Salary;
				}
			foreach (BaseDepartment d in SubDepartments(deptID))
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
			BaseDepartment d = DepartmentsTable.GetDepartment(startDeptID);
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
			{
				UpdateUpperDeptSalaries(worker.DeptID, worker.Salary);
				GetDepartment(worker.DeptID).NumberOfEmployees++;
			}
			return result;
		}

		/// <summary>
		/// Creates new worker of specified type and adds him to the organization
		/// </summary>
		/// <param name="fn">First name</param>
		/// <param name="ln">Last name</param>
		/// <param name="dob">Date of birth</param>
		/// <param name="deptID">Department ID</param>
		/// <param name="pos">Position</param>
		/// <returns>
		///  0 if worker was added successfully
		/// -1 if worker with same ID already exists which should not happen in principle
		/// because worker is newly created 
		/// </returns>
		public int AddWorker(string fn, string ln, DateTime dob, uint deptID, Positions pos)
		{
			Worker newWorker = null;
			switch(pos)
			{
				case Positions.President:
					newWorker = new Director(fn, ln, dob, DateTime.Now, deptID, "President", pos);
					break;
				case Positions.VicePresident:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, deptID, "VicePresident", pos);
					break;
				case Positions.DivisionHead:
					newWorker = new Director(fn, ln, dob, DateTime.Now, deptID, 
						"Head of the " + GetDepartment(deptID).DeptName, pos);
					break;
				case Positions.ViceDivisionHead:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, deptID,
						"Deputy Head of the " + GetDepartment(deptID).DeptName, pos);
					break;
				case Positions.DeptDirector:
					newWorker = new Director(fn, ln, dob, DateTime.Now, deptID, 
						"Director" + GetDepartment(deptID).DeptName.Substring(10), pos);
					break;
				case Positions.ViceDeptDirector:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, deptID,
						"Vice Director" + GetDepartment(deptID).DeptName.Substring(10), pos);
					break;
				case Positions.Employee:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, deptID, "Employee", pos);
					break;
				case Positions.Intern:
					newWorker = new Intern(fn, ln, dob, DateTime.Now, deptID, "Intern");
					break;
				default:
					break;
			}
			return AddWorker(newWorker);
		}

		public Worker GetWorker(uint workerID)
		{
			return WorkersTable.GetWorker(workerID);
		}

		public Director GetDirector(uint deptID)
		{
			return WorkersTable.GetDirector(deptID);
		}

		public List<PositionsTuple> AvailablePositionsList()
		{
			return WorkersTable.AvailablePositionsList();
		}

		public List<Worker> OneDepartmentWorkersList(uint deptID)
		{
			return WorkersTable.OneDepartmentWorkersList(deptID);
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

		public BaseDepartment GetDepartment(uint deptID)
		{
			return DepartmentsTable.GetDepartment(deptID);
		}

		public BaseDepartment GetRootDepartment()
		{
			return DepartmentsTable.GetRootDepartment();
		}

		public uint GetRootDeptID()
		{
			return DepartmentsTable.GetRootDeptID();
		}

		/// <summary>
		/// Returns a copy of list of departments
		/// </summary>
		/// <returns></returns>
		public List<BaseDepartment> GetDepartmentsList()
		{
			return DepartmentsTable.GetDepartmentsList();
		}
		public List<BaseDepartment> SubDepartments(uint deptID)
		{
			return DepartmentsTable.SubDepartments(deptID);
		}

		public int AddDepartment(BaseDepartment parentDept, BaseDepartment childDept)
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
