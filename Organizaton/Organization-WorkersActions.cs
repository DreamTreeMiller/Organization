using MLM.Interfaces;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Organizaton
{
	/// <summary>
	/// Implementation of manipulations on workers
	/// </summary>
	public partial class Organization : IWorkersActions
	{
		/// <summary>
		/// Creates new worker of specified type and adds him to the organization
		/// </summary>
		/// <param name="fn">First name</param>
		/// <param name="ln">Last name</param>
		/// <param name="dob">Date of birth</param>
		/// <param name="d">Department interface</param>
		/// <param name="pos">Position</param>
		/// <returns>
		///  0 if worker was added successfully, 
		/// -1 if worker is director and department already has a director
		/// </returns>
		public int AddWorker(string fn, string ln, DateTime dob, IDepartmentDTO d, Positions pos)
		{
			// Check if we are not adding a director to the department which already has a director
			if((pos == Positions.President ||
				pos == Positions.DivisionHead ||
				pos == Positions.DeptDirector) &&
				Director(d) != null)
				return -1;

			Worker newWorker = null;
			switch (pos)
			{
				case Positions.President:
					newWorker = new Director(fn, ln, dob, DateTime.Now, d.DeptID, "President", pos);
					break;
				case Positions.VicePresident:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, d.DeptID, "VicePresident", pos);
					break;
				case Positions.DivisionHead:
					newWorker = new Director(fn, ln, dob, DateTime.Now, d.DeptID,
						"Head of the " + d.DeptName, pos);
					break;
				case Positions.ViceDivisionHead:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, d.DeptID,
						"Deputy Head of the " + d.DeptName, pos);
					break;
				case Positions.DeptDirector:
					newWorker = new Director(fn, ln, dob, DateTime.Now, d.DeptID,
						"Director" + d.DeptName.Substring(10), pos);
					break;
				case Positions.ViceDeptDirector:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, d.DeptID,
						"Vice Director" + d.DeptName.Substring(10), pos);
					break;
				case Positions.Employee:
					newWorker = new Employee(fn, ln, dob, DateTime.Now, d.DeptID, "Employee", pos);
					break;
				case Positions.Intern:
					newWorker = new Intern(fn, ln, dob, DateTime.Now, d.DeptID, "Intern");
					break;
				default:
					break;
			}
			return AddWorker(newWorker as IWorker);
		}

		/// <summary>
		/// Adds worker to the organization workers' list. 
		/// Worker must have valid dept ID. 
		/// Number of workers of the specified department is increased by 1
		/// </summary>
		/// <param name="worker">worker</param>
		/// <returns>
		///  0 if worker was added successfully
		/// -1 if worker with same ID already exists
		/// -2 if director is already exists
		/// </returns>
		public int AddWorker(IWorker worker)
		{
			// Check if a being added worker does not have same ID as someone else
			if (Workers.Find(w => w.ID == worker.ID) != null) return -1;

			var d = Department(worker.DeptID);
			Workers.Add(worker as Worker);
			d.NumberOfEmployees++;
			UpdateSalaries(d as BaseDepartment);
			return 0;
		}

		/// <summary>
		/// Updates worker with new data from
		/// </summary>
		/// <param name="">Updated worker</param>
		/// <returns>
		///  0 if worker was updated successfully
		/// -1 if worker with such ID was not found
		/// </returns>
		public int EditWorker(IWorkerDTO updatedW)
		{
			// Get index of the being updated worker
			int wi = Workers.FindIndex(w => w.ID == updatedW.ID);

			// Check if worker type was changed
			// In this case need to "cast" new type
			// It is not real casting of a type, but creating instance of new type 
			// and copiying proper fields.
			// Creation of an instance of new type is done through specific constructor
			// which copies readonly unique worker ID

			Worker newW = new Intern(updatedW);

			//if (updatedW.OriginalWorkerType != updatedW.SelectedWorkerType)
			//{
				// Here new Director(updatedW), new Employee(..), new Intern(..)
				// Create new type and update salary
				switch (updatedW.SelectedWorkerType)
					{
						case WorkerHierarchy.Director:
							newW = new Director(updatedW);
							break;
						case WorkerHierarchy.Employee:
							newW = new Employee(updatedW);
							(newW as Employee).HourlyRate  = updatedW.HourlyRate;
							(newW as Employee).HoursWorked = updatedW.HoursWorked;
							break;
						case WorkerHierarchy.Intern:
							newW.Salary = updatedW.IntSalary;
							break;
					}
			//}
			//else
			//{
			//	// Only update salary
			//	switch (updatedW.SelectedWorkerType)
			//	{
			//		case WorkerHierarchy.Director:
			//			break;
			//		case WorkerHierarchy.Employee:
			//			(newW as Employee).HourlyRate  = updatedW.HourlyRate;
			//			(newW as Employee).HoursWorked = updatedW.HoursWorked;
			//			break;
			//		case WorkerHierarchy.Intern:
			//			newW.Salary = updatedW.IntSalary;
			//			break;
			//	}
			//}

			Workers[wi] = newW;

			// Update worker's properties
			Workers[wi].Position		= updatedW.Position;
			Workers[wi].PositionTitle	= updatedW.PositionTitle;

			var d = Department(updatedW.DeptID);
			UpdateSalaries(d as BaseDepartment);

			return 0;           // Worker was updated successfully
		}

		/// <summary>
		/// Moves worker to the department with specified ID
		/// </summary>
		/// <param name="worker">Worker to move</param>
		/// <param name="destDept">Destination department</param>
		/// <returns>
		/// 0 if moved successfully
		/// -1 if worker with such ID does not exist
		/// -2 if department with such ID does not exist
		/// -3 if destination department is current worker's department
		/// </returns>
		public int MoveWorker(IWorker worker, IDepartmentDTO destDept)
		{
			// Check if worker with such ID exists
			var w = RemoveWorker(worker);
			if (w == null) return -1;

			// Check if destination departmetn exists
			var newDept = Department(destDept.DeptID);
			if (newDept == null) return -2;

			uint oldDeptID = w.DeptID;

			// Check if destination department is not current worker's department
			if (destDept.DeptID == oldDeptID) return -3;

			// Worker can be moved to another department only either as Employee or Intern
			if (w.Position != Positions.Employee &&
				w.Position != Positions.Intern)
			{
				w.Position = Positions.Employee;
				w.PositionTitle = "Employee";
			}

			// Move worker to destination department
			w.DeptID = destDept.DeptID;

			if (w is Director)
			{
				// Change type of w from Director to Employee
				// Since there is not explicit type cast, 
				// need to create new instace of Employee class with same ID
				Employee e = new Employee(w as Worker);
				AddWorker(e);
			}
			else
				AddWorker(w);
			return 0;
		}

		/// <summary>
		/// Completely removes worker with specified ID from the list
		/// </summary>
		/// <param name="w">Worker to remove</param>
		/// <returns>
		/// Removed worker, if he was removed successfully, 
		/// null if the worker was not found
		/// </returns>
		public IWorker RemoveWorker(IWorker w)
		{
			bool result = Workers.Remove(w as Worker);
			if (result)
			{
				var d = Department(w.DeptID);
				d.NumberOfEmployees--;
				UpdateSalaries(d as BaseDepartment);
				return w;
			}
			return null;
		}
	}
}
