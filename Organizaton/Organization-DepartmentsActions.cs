using MLM.Interfaces;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Organizaton
{
	public partial class Organization : IDepartmentsActions
	{
		/// <summary>
		/// Adds newly created child department to parent department
		/// </summary>
		/// <param name="parentDept">Parent department</param>
		/// <param name="childDept">Departmet to add</param>
		/// <returns>
		///  0 if child was added successfully, 
		/// </returns>
		public int AddDepartment(IDepartmentDTO pd, IDepartmentDTO cd)
		{
			BaseDepartment parentDept = pd as BaseDepartment;
			BaseDepartment childDept  = cd as BaseDepartment;

			// Creation of root department
			// it means childDept is root department
			if (parentDept == null && childDept.ParentDeptID == 0)
			{
				Departments.Add(childDept);
				return 0;
			}

			// Adding child department to parent dept
			Departments.Add(childDept);
			parentDept.SubDepts.Add(childDept.DeptID);
			childDept.ParentDeptID = parentDept.DeptID;
			return 0;
		}

		/// <summary>
		/// Adds newly created child department to parent department
		/// </summary>
		/// <param name="pd">Parent department</param>
		/// <param name="childDeptName">Name of the department to be created</param>
		/// <param name="childDeptCreationDate">Child department creation date</param>
		public IDepartmentDTO CreateDepartment(IDepartmentDTO pd, string childDeptName, DateTime childDeptCreationDate)
		{
			BaseDepartment parentDept = pd as BaseDepartment;
			BaseDepartment childDept;
			// Creation of root department
			// it means childDept is root department
			if (parentDept == null)
			{
				childDept = new HQ(childDeptName);
				childDept.CreatedOn = childDeptCreationDate;
				Departments.Add(childDept);
				return childDept as IDepartmentDTO;
			}

			// Create Division level department
			uint rootDeptID = RootDepartment().DeptID;
			if (parentDept.DeptID == rootDeptID)
			{
				childDept = new Division(childDeptName, rootDeptID)
				{
					CreatedOn = childDeptCreationDate
				};
				Departments.Add(childDept);
				parentDept.SubDepts.Add(childDept.DeptID);
				childDept.ParentDeptID = parentDept.DeptID;
				return childDept as IDepartmentDTO;
			}

			// Create Department level department
			childDept = new Department(childDeptName, parentDept.DeptID)
			{
				CreatedOn = childDeptCreationDate
			};
			Departments.Add(childDept);
			parentDept.SubDepts.Add(childDept.DeptID);
			childDept.ParentDeptID = parentDept.DeptID;
			return childDept as IDepartmentDTO;
		}

		/// <summary>
		/// Updates name and creation date of the department
		/// </summary>
		/// <param name="deptToEdit">Department to edit</param>
		/// <param name="newName">New department name</param>
		/// <param name="newCreationDate">New creation date</param>
		/// <returns>
		/// 0 if department was updated successfully
		/// </returns>
		public int EditDepartment(IDepartmentDTO deptToEdit, string newName, DateTime newCreationDate)
		{
			deptToEdit.DeptName  = newName;
			deptToEdit.CreatedOn = newCreationDate;
			return 0;
		}

		/// <summary>
		/// Temporary container for AvailableDestinationDepartmentsList result outside the method 
		/// because it uses recursion
		/// </summary>
		private List<IDepartmentDTO> _availDestDeptList;

		/// <summary>
		/// Returns a list of departments where we can move current department because
		/// department cannot be moved to any its direct or distant subdepartments 
		/// </summary>
		/// <param name="d">Department to be moved</param>
		/// <returns>List of available departments to move specified one</returns>
		public List<IDepartmentDTO> AvailableDestinationDepartmentsList(IDepartmentDTO d)
		{
			_availDestDeptList = DepartmentsList();
			TakeOutSubDeptsAndDept(d);
			_availDestDeptList.Remove(Department(d.ParentDeptID));
			return _availDestDeptList;
			
			void TakeOutSubDeptsAndDept(IDepartmentDTO dd)
			{
				foreach (var sd in SubDepartments(dd))
						TakeOutSubDeptsAndDept(sd);
				_availDestDeptList.Remove(dd);
			}
		}

		/// <summary>
		/// Moves department to destination department
		/// </summary>
		/// <param name="d">Department to move</param>
		/// <param name="destinationDept">Destination department to nest in as subdepartment</param>
		/// <returns>Moved department with updated parent dept ID</returns>
		public IDepartmentDTO MoveDepartment(IDepartmentDTO d, IDepartmentDTO destinationDept)
		{
			var cloneDept = d as BaseDepartment;
			var oldParent = Department(d.ParentDeptID) as BaseDepartment;

			// Take out being moved department from parent's subdepartments
			oldParent.SubDepts.Remove(d.DeptID);

			// If current department level is Division, it means we are going down to Department level
			// and need to change class of department
			if (d is Division)
			{
				cloneDept = new Department(d as BaseDepartment);
			}

			// If we are moving up to Division level, 
			// need to change class from Department to Division
			if (destinationDept.ParentDeptID == 0)
			{
				cloneDept = new Division(d as BaseDepartment);
				cloneDept.DeptName = "Division_" + UniqueID.Name();
			}
			else if (SubDepartments(destinationDept).
					Find(sameName => sameName.DeptName == cloneDept.DeptName) != null)
					cloneDept.DeptName += "_" + UniqueID.Name();

			// Set new parent for being moved department
			cloneDept.ParentDeptID = destinationDept.DeptID;

			// Add new child to destination subdepartment
			(destinationDept as BaseDepartment).SubDepts.Add(cloneDept.DeptID);

			// If class of department changed, need to update Departments table
			int di = Departments.FindIndex(dept => dept.DeptID == d.DeptID);
			Departments[di] = cloneDept;

			UpdateSalaries(oldParent);
			UpdateSalaries(destinationDept as BaseDepartment);
			return cloneDept;
		}


		/// <summary>
		/// Deletes department which has no employees and sub departments
		/// </summary>
		/// <param name="d">Department to delete</param>
		/// <returns>
		/// Deleted department
		/// null if such department does not exist or we are trying to delete root department
		/// </returns>
		public IDepartmentDTO DeleteEmptyDepartment(IDepartmentDTO d)
		{
			bool result = Departments.Remove(d as BaseDepartment);
			if (result)
			{
				BaseDepartment parent = Department(d.ParentDeptID) as BaseDepartment;
				parent.SubDepts.Remove(d.DeptID);
				return d;
			}
			return null;
		}

		/// <summary>
		/// Removes department with specified ID from the list of departments
		/// provided that this is not root department.
		/// Adds sub departments to the parent department.
		/// Adds all department workers to the parent department.
		/// </summary>
		/// <param name="d">Department to delete</param>
		/// <returns>
		/// Deleted department
		/// null if such department does not exist or we are trying to delete root department
		/// </returns>
		public IDepartmentDTO SelfExcludeOfDepartment(IDepartmentDTO d)
		{
			var parentDept = Department(d.ParentDeptID) as BaseDepartment;

			var dir = Director(d);
			if (dir != null) MoveWorker(dir, parentDept);

			// update department ID of each worker to destinationDept ID
			foreach (var w in OneDepartmentWorkersList(d))
			{
				w.DeptID = parentDept.DeptID;

				// Make all vice bosses as Employees
				if (w.Position != Positions.Employee ||
					w.Position != Positions.Intern)
				{
					w.Position = Positions.Employee;
					w.PositionTitle = "Employee";
				}
			}
			d.NumberOfEmployees = 0;

			foreach (var dept in SubDepartments(d))
				MoveDepartment(dept, parentDept);

			UpdateSalaries(parentDept);
			return DeleteEmptyDepartment(d);
		}
	
		/// <summary>
		/// Deletes department including all sub departments with all employees
		/// </summary>
		/// <param name="d">Department to delete</param>
		public void DeleteCompletely(IDepartmentDTO d)
		{
			int i = 0;
			while (i < Workers.Count)
			{
				// We should NOT increment i after taking out a worker
				// because next worker has the same index as just deleted one
				if (Workers[i].DeptID == d.DeptID)
				{
					Workers.RemoveAt(i);
				}
				else
					i++;
			}

			// Check if d has sub departments
			if ((d as BaseDepartment).SubDepts.Count > 0)
			{
			// Delete all subdepartments from Departments list
			// Let's check deptID of each department if it belongs to SubDepts list
			// If DeptID is in SubDept list - delete department and ID from the SubDept list
			// When there is no SubDepts remain stop scanning Departments
				for (i = 0; i < Departments.Count; i++)
					if ((d as BaseDepartment).SubDepts.Count > 0)
					{
						int j = 0;
						while (j < (d as BaseDepartment).SubDepts.Count)
						{
							if (Departments[i].DeptID == (d as BaseDepartment).SubDepts[j])
							{
								DeleteCompletely(Departments[i]);
								(d as BaseDepartment).SubDepts.RemoveAt(j);
								// After deleting subdepartment i will reman the same
								// but Department[i] will be different
								// because there will be shift of all list items to position i
							}
							else
								j++;
						}
					}
					else break; // if no mor subdepartments in  
			}
			// if no, or all were deleted, delete department
			Departments.Remove(d as BaseDepartment);
		}
	}
}
