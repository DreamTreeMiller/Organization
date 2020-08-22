﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class WorkersTable : IWorkers
	{
		/// <summary>
		/// Collection (DataBase table) of all workers of the organization
		/// </summary>
		public List<Worker> Workers { get; set; }

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
			if (Workers.Contains(worker)) return -1;       // Worker with such ID already works in dept
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
			int wi = Workers.FindIndex(w => w.ID == workerID);
			if (wi != -1)
				return Workers[wi];
			return null;
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
			int wi = Workers.FindIndex(w => w.DeptID == deptID && (w is Director));
			if (wi != -1)
				return Workers[wi] as Director;
			return null;
		}
	
		/// <summary>
		/// Finds all workers who work in the same department with deptID
		/// </summary>
		/// <param name="deptID">ID of the department</param>
		/// <returns>
		/// Collection of workers who works in the department with deptID
		/// </returns>
		public List<Worker> DepartmentWorkersList(uint deptID)
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