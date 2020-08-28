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

		public PositionsTable		PositionsTable { get; }

		/// <summary>
		/// Constructor. Creates root department with 0 as parent dept ID, 
		/// and with 0 workers and 0 subdepartments. Adds it to the collection of all departments
		/// </summary>
		/// <param name="orgName">Name of the organization. It is assigned to the root dept name.</param>
		public Organization (string orgName) 
		{
			Random r = new Random();
			BaseDepartment root	= new HQ(orgName);
			root.CreatedOn		= new DateTime(r.Next(2000, 2020), r.Next(1, 13), r.Next(1, 29));
			DepartmentsTable	= new DepartmentsTable();
			WorkersTable		= new WorkersTable();
			PositionsTable		= new PositionsTable();
			AddDepartment(null, root);
		}

		#region Salary manipulations

		///// <summary>
		///// Calculates total salary of specified department by calculating salaries of:
		///// - employees, inters
		///// - sub departments
		///// - director whose salary is calculated based on dept employees' and sub departments' salaries
		///// </summary>
		///// <param name="deptID">Department ID</param>
		///// <param name="pymnt">Type of payment: 
		///// Standard - all employees work same number of hours.
		///// Random - each employee is assigned with a random number of hours between 100 and 240.
		///// </param>
		///// <returns>Total salary of the specified department</returns>
		//public int CalculateTotalDeptSalary(uint deptID, PaymentType pymnt)
		//{
		//	int TotalDeptSal = 0;
		//	foreach (Worker w in OneDepartmentWorkersList(deptID))
		//		if (!(w is Director))
		//		{
		//			w.Salary = (pymnt == PaymentType.Standard) ?
		//												22 * 8 :    // Standard 8 hours per 22 working days
		//									   r.Next(100, 240);    // Random selection of hours
		//			TotalDeptSal += w.Salary;
		//		}
		//	foreach (BaseDepartment d in SubDepartments(deptID))
		//		TotalDeptSal += CalculateTotalDeptSalary(d.DeptID, pymnt);

		//	Director dir = GetDirector(deptID);
		//	dir.Salary = TotalDeptSal / 100 * 15;
		//	//UpdateEmployee(dir);
		//	TotalDeptSal += dir.Salary;
		//	GetDepartment(deptID).TotalDepartmentSalary = TotalDeptSal;
		//	return TotalDeptSal;
		//}

		/// <summary>
		/// Updates total department and director's salary of specified department
		/// and all parent departments up to the root
		/// It assumes that first salary change happened in the child department's salary
		/// </summary>
		/// <param name="startDeptID">ID of starting department</param>
		/// <param name="newTotalDeptSalary">Salary difference either positive or negative</param>
		//private void UpdateUpperDeptSalaries(uint startDeptID, int salaryDiff)
		//{
		//	BaseDepartment d = DepartmentsTable.GetDepartment(startDeptID);
		//	Director dir = WorkersTable.GetDirector(startDeptID);

		//	// We are changing this now!!
		//	// Important!!! This method is called when a worker is already updated, added or deleted)
		//	// but Total Department Salary is still NOT updated!
		//	int currTotalDeptSal = d.TotalDepartmentSalary;
		//	int newSubDeptAndWorkersSalary = currTotalDeptSal + salaryDiff;

		//	if (dir != null)
		//	{
		//		// ==> if director present - one way to update salary

		//		// Get new total workers and sub-dept salaries in order to calculate new salary of dept boss
		//		newSubDeptAndWorkersSalary -= dir.Salary;
		//		dir.Salary = newSubDeptAndWorkersSalary / 100 * 15;
		//		d.TotalDepartmentSalary = newSubDeptAndWorkersSalary + dir.Salary;
		//	}
		//	else
		//	{
		//		// ==> if not another - new total department salary is already calculated
		//		d.TotalDepartmentSalary = newSubDeptAndWorkersSalary;
		//	}
		//	// Parent is reached. Calculation is completed
		//	if (startDeptID == 0) return;

		//	UpdateUpperDeptSalaries(d.ParentDeptID, d.TotalDepartmentSalary - currTotalDeptSal);
		//}

		/// <summary>
		/// Updates salaries of current department and all upper departments.
		/// Not so efficient, but simple. If there is no time constraints, it's Ok
		/// </summary>
		/// <param name="dept">Department which workers salaries must be updated, and all above</param>
		private void UpdateSalaries(BaseDepartment dept)
		{
			// Collect salaries of all sub departments
			dept.TotalSubDepartmentsSalary = 
				dept.SubDepts.Sum(d =>
				{
					var sd = GetDepartment(d);
					int sdSalary = 0;
					if (sd != null) sdSalary = sd.TotalDepartmentSalary;
					return sdSalary;
				});

			// Calculate total salay of all staff except director
			var dir = GetDirector(dept.DeptID);
			int dirSalary = 0;
			dept.TotalDeptStaff_withoutBoss_Salary = 
				OneDepartmentWorkersList(dept.DeptID).Sum(w => w.Salary);

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
			UpdateSalaries(GetDepartment(dept.ParentDeptID));
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
		/// -2 if director is already exists
		/// </returns>
		public int AddWorker(Worker worker)
		{
			// Check if we are not adding a director to the department which already has a director
			if ((worker is Director) &&
				GetDirector(worker.DeptID) != null)
				return -2;

			int result = WorkersTable.AddWorker(worker);

			// Check if a being added worker does not have same ID as someone else
			if (result == 0)
			{
				var d = GetDepartment(worker.DeptID);
				d.NumberOfEmployees++;
				UpdateSalaries(d);
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
		public int CreateWorker(string fn, string ln, DateTime dob, uint deptID, Positions pos)
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

		/// <summary>
		/// Finds worker with specified ID
		/// </summary>
		/// <param name="workerID">Worker's ID</param>
		/// <returns>
		/// Worker with specified ID, 
		/// null if worker with such ID does not exist
		/// </returns>
		public Worker GetWorker(uint workerID)
		{
			return WorkersTable.GetWorker(workerID);
		}

		/// <summary>
		/// Finds director (president,head) of deptID department
		/// </summary>
		/// <param name="deptID">Department ID</param>
		/// <returns>
		/// Worker of the Director class or 
		/// null if a director of deptID department is not found
		/// </returns>
		public Director GetDirector(uint deptID)
		{
			return WorkersTable.GetDirector(deptID);
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
		/// -3 if destination department is current worker's department
		/// </returns>
		public int MoveWorker(uint workerID, uint destinationDeptID)
		{
			// Check if worker with such ID exists
			Worker w = RemoveWorker(workerID);
			if (w == null) return -1;

			// Check if destination departmetn exists
			var newDept = GetDepartment(destinationDeptID);
			if (newDept == null) return -2;

			uint oldDeptID = w.DeptID;

			// Check if destination department is not current worker's department
			if (destinationDeptID == oldDeptID) return -3;

			// The oder below is very important.
			// First we move worker to new department, then update departments


			// Move worker to destination department
			w.DeptID = destinationDeptID;

			if (w is Director)
				AddWorker(w as Employee);
			else
				AddWorker(w);
			return 0;
		}

		/// <summary>
		/// Returns list of available positions (position, position name string)
		/// </summary>
		/// <returns></returns>
		public List<PositionsTuple> AvailablePositionsList(BaseDepartment d)
		{
			if (d == null) return null;
			var availablePositionsList = PositionsTable.Available(d);
			return availablePositionsList;
		}

		/// <summary>
		/// Finds all workers, including director, who work in the same department with deptID
		/// and puts them in the collection List
		/// </summary>
		/// <param name="deptID">ID of the department</param>
		/// <returns>Collection of workers who works in the department with deptID</returns>
		public List<Worker> OneDepartmentWorkersList(uint deptID)
		{
			return WorkersTable.OneDepartmentWorkersList(deptID);
		}

		/// <summary>
		/// Completely removes worker with specified ID from the list
		/// </summary>
		/// <param name="workerID">Employee ID</param>
		/// <returns>
		/// Removed worker, if he was removed successfully, 
		/// null if the worker was not found
		/// </returns>
		public Worker RemoveWorker(uint workerID)
		{
			Worker w = WorkersTable.RemoveWorker(workerID);
			if (w != null)
			{
				var d = GetDepartment(w.DeptID);
				d.NumberOfEmployees--;
				UpdateSalaries(d);
			}
			return w;
		}

		#endregion

		#region Implementation of IDepartments interface

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
		
		// Not sure if I need this method
		//public List<BaseDepartment> SubDepartments(uint deptID)
		//{
		//	return DepartmentsTable.SubDepartments(deptID);
		//}

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
