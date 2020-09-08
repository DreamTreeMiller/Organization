using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class WorkersTable
	{
		/// <summary>
		/// Finds worker with specified ID
		/// </summary>
		/// <param name="workerID">Worker's ID</param>
		/// <returns>
		/// Worker with specified ID, 
		/// null if worker with such ID does not exist
		/// </returns>
		//public Worker GetWorker(uint workerID) 
		//{
		//	return Workers.Find(w => w.ID == workerID);
		//}

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
		//		Workers[wi].PositionTitle	= worker.PositionTitle;
		//		Workers[wi].Position	= worker.Position;
		//		Workers[wi].salaryBase	= worker.salaryBase;
		//		return 0;           // Worker was updated successfully
		//	}
		//	return -1;              // No such worker in the dept
		//}

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
