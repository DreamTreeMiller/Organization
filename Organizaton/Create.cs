using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	static class Create
	{
		public static Random r = new Random();

		public static Organization Organization(int maximumDepth,
												int maximumSubDepts,
												string orgName,
												int maximumNumOfWorkersInDept)
		{
			Organization orgToCreate = new Organization(orgName);
			CreateRandomOrganization(maximumDepth,
									 maximumSubDepts,
									 "",
									 maximumNumOfWorkersInDept,
									 orgToCreate.GetRootDeptID(),
									 Hierarchy.Top);
			return orgToCreate as Organization;

			/// <summary>
			/// Generates random workers and subdepts
			/// </summary>
			/// <param name="maxDepth">1 - no sub departments, 2 - top and 1 level of departments, etc.</param>
			/// <param name="maxSubDepts">Maximum number of subdepartments on next level</param>
			/// <param name="deptNameCode">Code to add to department name "4", "53", "2117", etc. Top most is "1".
			/// First call of the method with "" code (empty string)</param>
			/// <param name="maxNumOfWorkersInDept">Maximum number of workers in the current deparment</param>
			/// <param name="CreatedDeptID">ID of just created department. 
			/// First call of the method with root dept ID</param>
			/// <param name="level">Top - level 1, Division - level 2, Department - level 3, 4 ...</param>
			void CreateRandomOrganization(int maxDepth,
										  int maxSubDepts,
										  string deptNameCode,
										  int maxNumOfWorkersInDept,
										  uint CreatedDeptID,
										  Hierarchy level)
			{
				// Creating a random list of employees of current department
				CreateDeptEmployees(maxNumOfWorkersInDept,
									level,
									CreatedDeptID,
									deptNameCode == "" ? orgName : deptNameCode);

				// if we still need to create sub-departments
				if (maxDepth > 1)
				{
					BaseDepartment parentD = orgToCreate.GetDepartment(CreatedDeptID);
					switch (level)
					{
						case Hierarchy.Top:
							level = Hierarchy.Division;
							for (int i = 1; i <= r.Next(3, maxSubDepts < 3 ? 4 : maxSubDepts + 1); i++)
							{
								BaseDepartment newDpt = new Division("Division" + $"_{i}", CreatedDeptID);
								orgToCreate.AddDepartment(parentD, newDpt);
								CreateRandomOrganization(maxDepth - 1,
														 maxSubDepts,
														 deptNameCode + $"_{i}",
														 maxNumOfWorkersInDept,
														 newDpt.DeptID,
														 level);
							}
							break;
						case Hierarchy.Division:
						case Hierarchy.Department:
							level = Hierarchy.Department;
							for (int i = 1; i <= r.Next(1, maxSubDepts < 1 ? 2 : maxSubDepts + 1); i++)
							{
								BaseDepartment newDpt = new Department("Department" + deptNameCode + $"_{i}", CreatedDeptID);
								orgToCreate.AddDepartment(parentD, newDpt);
								CreateRandomOrganization(maxDepth - 1,
														 maxSubDepts,
														 deptNameCode + $"_{i}",
														 maxNumOfWorkersInDept,
														 newDpt.DeptID,
														 level);
							}
							break;
					}
				}
			}

			void CreateDeptEmployees(int maxNumOfWorkersInDept,
										Hierarchy level,
										uint deptID,
										string deptNameCode)
			{
				string posHeadStr = "";
				string posViceHeadStr = "";
				Positions posHead = Positions.DeptDirector;
				Positions posViceHead = Positions.ViceDeptDirector;
				DateTime deptCD = orgToCreate.GetDepartment(deptID).CreatedOn;
				switch (level)
				{
					case Hierarchy.Top:
						maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 7) ? 7 : r.Next(7, maxNumOfWorkersInDept);
						posHead = Positions.President;
						posHeadStr = "President";
						posViceHead = Positions.VicePresident;
						posViceHeadStr = "Vice President";
						break;
					case Hierarchy.Division:
						maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 5) ? 5 : r.Next(5, maxNumOfWorkersInDept);
						posHead = Positions.DivisionHead;
						posHeadStr = "Head of the Division_" + deptNameCode;
						posViceHead = Positions.ViceDivisionHead;
						posViceHeadStr = "Deputy Head of the Division_" + deptNameCode;
						break;
					case Hierarchy.Department:
						maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 3) ? 3 : r.Next(3, maxNumOfWorkersInDept);
						posHead = Positions.DeptDirector;
						posHeadStr = "Director_" + deptNameCode;
						posViceHead = Positions.ViceDeptDirector;
						posViceHeadStr = "Vice Director_" + deptNameCode;
						break;
				}

				int numOfEmployees = r.Next(1, maxNumOfWorkersInDept - 2 + 1);
				int numOfInterns = maxNumOfWorkersInDept - 2 - numOfEmployees;

				// Adding head of the organization/division/department
				orgToCreate.AddWorker(new Director("First_" + UniqueID.Name(),
										 "Last_" + UniqueID.Name(),
										 new DateTime(r.Next(1950, 1981), r.Next(1, 13), r.Next(1, 29)),
										 new DateTime(r.Next(deptCD.Year, 2020),
													  r.Next(deptCD.Month, 13),
													  r.Next(1, 29)),
										 deptID,
										 posHeadStr,
										 posHead));
				// Adding vice head
				orgToCreate.AddWorker(new Employee("First_" + UniqueID.Name(),
										 "Last_" + UniqueID.Name(),
										 new DateTime(r.Next(1950, 1981), r.Next(1, 13), r.Next(1, 29)),
										 new DateTime(r.Next(deptCD.Year, 2020),
													  r.Next(deptCD.Month, 13),
													  r.Next(1, 29)),
										 deptID,
										 posViceHeadStr,
										 posViceHead));
				// Adding employees
				for (int i = 1; i <= numOfEmployees; i++)
					orgToCreate.AddWorker(new Employee("First_" + UniqueID.Name(),
											 "Last_" + UniqueID.Name(),
											 new DateTime(r.Next(1950, 1996), r.Next(1, 13), r.Next(1, 29)),
											 new DateTime(r.Next(deptCD.Year, 2020),
														  r.Next(deptCD.Month, 13),
														  r.Next(1, 29)),
											 deptID,
											 "Employee"));
				// Adding interns
				for (int i = 1; i <= numOfInterns; i++)
					orgToCreate.AddWorker(new Intern("First_" + UniqueID.Name(),
											 "Last_" + UniqueID.Name(),
											 new DateTime(r.Next(1990, 2003), r.Next(1, 13), r.Next(1, 29)),
											 new DateTime(r.Next(deptCD.Year, 2020),
														  r.Next(deptCD.Month, 13),
														  r.Next(1, 29)),
											 deptID,
											 "Intern"));
			}

		}
	}
}
