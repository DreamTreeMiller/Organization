using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Organization : Department
	{
		public new static DateTime CreatedOn;
		public Organization (string orgName) 
				: base (orgName, null)
		{
			Random r = new Random();
			CreatedOn = new DateTime(r.Next(2000, 2020), r.Next(1, 13), r.Next(1, 29));
		}
		public int MoveEmployee(ulong empID, Department origin, Department destination)
		{
			Worker w = origin.GetEmployee(empID);
			if (w == null) return -1;       // No worker with such ID works in origin dept
			w.DepName = destination.Name;
			destination.AddEmployee(w);
			return 0;
		}

		/// <summary>
		/// Generates random workers and subdepts
		/// </summary>
		/// <param name="maxDepth">1 - no sub departments, 2 - top and 1 level of departments, etc.</param>
		/// <param name="maxSubDepts">Maximum number of subdepartments on next level</param>
		/// <param name="deptNameCode">Code to add to department name "4", "53", "2117", etc. Top most is "1".
		/// First call of the method with "" code (empty string)</param>
		/// <param name="maxNumOfWorkersInDept">Maximum number of workers in the current deparment</param>
		/// <param name="parentDept">Parent department. First call of the method with 'null'</param>
		/// <param name="level">Top - level 1, Division - level 2, Department - level 3, 4 ...</param>
		public void CreateRandomOrganization   (int maxDepth, 
												int maxSubDepts, 
												string deptNameCode,
												int maxNumOfWorkersInDept, 
												Department parentDept,
												Hierarchy level)
		{
			Random r = new Random();
			int numOfDeptsAtThisLevel = 0;
			List<Worker> newEmployeesList = CreateEmployeesList(maxNumOfWorkersInDept,
																level,
																level == Hierarchy.Top ? this : parentDept,
																deptNameCode == "" ? this.Name : deptNameCode);
			if (parentDept == null)
			{ 
				this.Employees = newEmployeesList; 
			}
			else
			{ 
				parentDept.Employees = newEmployeesList;
			}
			switch (level)
			{
				case Hierarchy.Top:
					numOfDeptsAtThisLevel = r.Next(3, maxSubDepts);
					level = Hierarchy.Division;
					break;
				default:
					numOfDeptsAtThisLevel = r.Next(0, maxSubDepts);
					level = Hierarchy.Department;
					break;
			}

			if (maxDepth > 1)
			{
				for (int i = 1; i <= numOfDeptsAtThisLevel; i++)
				{
					Department newDpt = new Department("Department"+ deptNameCode + $"_{i}", this);
					if (parentDept == null)
					{
						this.AddDepartment(newDpt);
					}
					else
					{
						parentDept.AddDepartment(newDpt);
					}
					CreateRandomOrganization(maxDepth - 1,
											 maxSubDepts,
											 deptNameCode + $"_{i}",
											 maxNumOfWorkersInDept,
											 newDpt,
											 level);
				}
			}
		}

		List<Worker> CreateEmployeesList(int maxNumOfWorkersInDept, 
										 Hierarchy level, 
										 Department department,
										 string deptNameCode)
		{
			Random r = new Random();
			Director head;
			Employee vicehead;
			List<Worker> empList = new List<Worker>();
			deptNameCode = level == Hierarchy.Top ? deptNameCode : "Department_" + deptNameCode;
			string job;
			Positions posHead = Positions.DeptDirector;
			Positions posViceHead = Positions.ViceDeptDirector;
			switch (level)
			{
				case Hierarchy.Top:
					maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 7) ? 7 : r.Next(7, maxNumOfWorkersInDept);
					posHead = Positions.President;
					posViceHead = Positions.VicePresident;
					break;
				case Hierarchy.Division:
					maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 5) ? 5 : r.Next(5, maxNumOfWorkersInDept);
					posHead = Positions.DivisionHead;
					posViceHead = Positions.ViceDivisionHead;
					break;
				case Hierarchy.Department:
					maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 3) ? 3 : r.Next(3, maxNumOfWorkersInDept);
					posHead = Positions.DeptDirector;
					posViceHead = Positions.ViceDeptDirector;
					break;
			}

			int numOfEmployees = r.Next(1, maxNumOfWorkersInDept - 2 + 1);
			int numOfInterns = maxNumOfWorkersInDept - 2 - numOfEmployees;

			empList.Add(new Director("First_" + UniqueID.Name(),
									 "Last_"  + UniqueID.Name(),
									 new DateTime(r.Next(1950, 1981), r.Next(1,13), r.Next(1,29)),
									 new DateTime(r.Next(CreatedOn.Year, 2020), 
												  r.Next(CreatedOn.Month, 13), 
												  r.Next(1, 29)),
									 department,
									 "Manager",
									 posHead));

			empList.Add(new Employee("First_" + UniqueID.Name(),
									 "Last_" + UniqueID.Name(),
									 new DateTime(r.Next(1950, 1981), r.Next(1, 13), r.Next(1, 29)),
									 new DateTime(r.Next(CreatedOn.Year, 2020),
												  r.Next(CreatedOn.Month, 13),
												  r.Next(1, 29)),
									 department,
									 "Assistan to Manager",
									 posViceHead));
			for (int i = 1; i < numOfEmployees; i++)

			return empList;
		}
	}
}
