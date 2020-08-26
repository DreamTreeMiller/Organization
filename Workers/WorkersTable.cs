using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class WorkersTable
	{
		/// <summary>
		/// List of position names 
		/// It is used in Add worker dialog window
		/// </summary>
		private static List<PositionsTuple> PositionsNames  = new List<PositionsTuple>()
		{
		new PositionsTuple() { Pos = Positions.President,			PositionName = "President"},
		new PositionsTuple() { Pos = Positions.VicePresident,		PositionName = "VicePresident"},
		new PositionsTuple() { Pos = Positions.DivisionHead,		PositionName = "Head of the Division"},
		new PositionsTuple() { Pos = Positions.ViceDivisionHead,	PositionName = "Deputy Head of the Division"},
		new PositionsTuple() { Pos = Positions.DeptDirector,		PositionName = "Director"},
		new PositionsTuple() { Pos = Positions.ViceDeptDirector,	PositionName = "Vice Director"},
		new PositionsTuple() { Pos = Positions.Employee,			PositionName = "Employee"},
		new PositionsTuple() { Pos = Positions.Intern,				PositionName = "Intern"}
		};

		/// <summary>
		/// Returns list of available positions (position, position name string)
		/// </summary>
		/// <returns></returns>
		public List<PositionsTuple> AvailablePositionsList()
		{
			return new List<PositionsTuple>(PositionsNames);
		}

		/// <summary>
		/// Collection (DataBase table) of all workers of the organization
		/// </summary>
		private List<Worker> Workers { get; set; }

		/// <summary>
		/// Constructor initializes Workers collection
		/// </summary>
		public WorkersTable()
		{
			Workers = new List<Worker>();
		}

		/// <summary>
		/// Adds new worker to the worker's list. Checks if a worker with the same ID already works in the company
		/// </summary>
		/// <param name="worker"></param>
		/// <returns>
		///  0 if worker was added successfully, 
		/// -1 if such worker already exists
		/// </returns>
		public int AddWorker(Worker worker)
		{
			if (Workers.Find(w => w.ID == worker.ID) != null) return -1;       // Worker with such ID already works in dept
			Workers.Add(worker);
			return 0;
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
			return Workers.Find(w => w.ID == workerID);

			// In the beginning I wrote as below
			//int wi = Workers.FindIndex(w => w.ID == workerID);
			//if (wi != -1)
			//	return Workers[wi];
			//return null;
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
			return Workers.Find(w => w.DeptID == deptID && (w is Director)) as Director;
		}

		/// <summary>
		/// Finds all workers who work in the same department with deptID
		/// and puts them in the collection List
		/// </summary>
		/// <param name="deptID">ID of the department</param>
		/// <returns>Collection of workers who works in the department with deptID</returns>
		public List<Worker> OneDepartmentWorkersList(uint deptID)
		{
			return Workers.FindAll(w => w.DeptID == deptID);
		}

		/// <summary>
		/// Updates worker with the specified ID
		/// </summary>
		/// <param name="workerID">ID of the worker to update</param>
		/// <param name="worker">Worker </param>
		/// <returns></returns>
		//public int UpdateWorker(uint workerID, Worker worker)
		//{
		//	if (Workers.Contains(worker))
		//	{
		//		// This IndexOf will work based on overridden Equals in Worker class
		//		int wi = Workers.IndexOf(worker);
		//		Workers[wi].FirstName	= worker.FirstName;
		//		Workers[wi].LastName	= worker.LastName;
		//		Workers[wi].DateOfBirth	= worker.DateOfBirth;
		//		Workers[wi].EmployedOn	= worker.EmployedOn;
		//		Workers[wi].DeptID		= worker.DeptID;
		//		Workers[wi].JobTitle	= worker.JobTitle;
		//		Workers[wi].Position	= worker.Position;
		//		Workers[wi].salaryBase	= worker.salaryBase;
		//		return 0;           // Worker was updated successfully
		//	}
		//	return -1;              // No such worker in the dept
		//}

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
			Worker w = GetWorker(workerID);
			if (w != null) Workers.Remove(w); 
			return w;    
		}

		private class CompareByPosition : IComparer<Worker>
		{
			public int Compare(Worker x, Worker y)
			{
				if (x == null && y == null) return 0;           // Сотрудники идентичны
				if (x == null) return -1;                       // x - null, y not null, y is greater
				if (y == null) return 1;                       // x is not null, y is null, x is greater
				return x.Position.CompareTo(y.Position);
			}
		}

		/// <summary>
		/// Компаратор для поля зарплата
		/// </summary>
		private class CompareBySalary : IComparer<Worker>
		{
			public int Compare(Worker x, Worker y)
			{
				if (x == null && y == null) return 0;           // Сотрудники идентичны
				if (x == null) return -1;                       // x - null, y not null, y is greater
				if (y == null) return 1;                       // x is not null, y is null, x is greater
				return x.Salary.CompareTo(y.Salary);
			}
		}
	}
}
