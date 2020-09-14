using MLM.Interfaces;
using MLM.InterfacesActions;
using MLM.Organizaton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	/// <summary>
	/// Class just to generate whole organization once
	/// </summary>
	public class CreateOrganization : ICreateOrganization
	{
		public static Random r = new Random();

		/// <summary>
		/// Generates ones whole organization with random workers and subdepts
		/// </summary>
		/// <param name="maxDepth">1 - no sub departments, 2 - top and 1 level of departments, etc.</param>
		/// <param name="maxSubDepts">Maximum number of subdepartments on next level</param>
		/// <param name="deptNameCode">Code to add to department name "4", "53", "2117", etc. Top most is "1".
		/// First call of the method with "" code (empty string)</param>
		/// <param name="maxNumOfWorkersInDept">Maximum number of workers in the current deparment</param>
		/// <returns>Created organization via IOrganization interface</returns>
		public IOrganization Organization(int maximumDepth,
										  int maximumSubDepts,
										  string orgName,
										  int maximumNumOfWorkersInDept)
		{
			Organization orgToCreate = new Organization(orgName);
			orgToCreate.EstablishedOn = new DateTime(r.Next(2000, 2010), r.Next(1, 13), r.Next(1, 29));
			BaseDepartment root = new HQ("HQ")
			{
				CreatedOn = orgToCreate.EstablishedOn
			};
			orgToCreate.Departments.Add(root);

			CreateRandomOrganization(maximumDepth,
									 maximumSubDepts,
									 "",
									 maximumNumOfWorkersInDept,
									 root,
									 Hierarchy.Top);
			return orgToCreate;


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
										  BaseDepartment CreatedDept,
										  Hierarchy level)
			{
				// Creating a random list of employees of current department
				CreateDeptEmployees(maxNumOfWorkersInDept,
									level,
									CreatedDept,
									level == Hierarchy.Top ? "HQ" : deptNameCode);

				// if we still need to create sub-departments
				if (maxDepth > 1)
				{
					switch (level)
					{
						case Hierarchy.Top:
							level = Hierarchy.Division;
							for (int i = 1; i <= r.Next(3, maxSubDepts < 3 ? 4 : maxSubDepts + 1); i++)
							{
								BaseDepartment newDept = new Division("Division" + $"_{i}", CreatedDept.DeptID);
								newDept.CreatedOn = GenRandomDate(root.CreatedOn, DateTime.Now);
								orgToCreate.AddDepartment(CreatedDept, newDept);
								CreateRandomOrganization(maxDepth - 1,
														 maxSubDepts,
														 $"_{i}",
														 maxNumOfWorkersInDept,
														 newDept,
														 level);
							}
							break;
						case Hierarchy.Division:
						case Hierarchy.Department:
							level = Hierarchy.Department;
							for (int i = 1; i <= r.Next(1, maxSubDepts < 1 ? 2 : maxSubDepts + 1); i++)
							{
								BaseDepartment newDept = new Department("Department" + deptNameCode + $"_{i}", CreatedDept.DeptID);
								newDept.CreatedOn = GenRandomDate(root.CreatedOn, DateTime.Now);
								orgToCreate.AddDepartment(CreatedDept, newDept);
								CreateRandomOrganization(maxDepth - 1,
														 maxSubDepts,
														 deptNameCode + $"_{i}",
														 maxNumOfWorkersInDept,
														 newDept,
														 level);
							}
							break;
					}
				}
			}

			void CreateDeptEmployees(int maxNumOfWorkersInDept,
										Hierarchy level,
										BaseDepartment dept,
										string deptNameCode)
			{
				string posHeadStr = "";
				string posViceHeadStr = "";
				Positions posHead = Positions.DeptDirector;
				Positions posViceHead = Positions.ViceDeptDirector;
				DateTime deptCD = dept.CreatedOn;
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
						posHeadStr = "Head of the Division" + deptNameCode;
						posViceHead = Positions.ViceDivisionHead;
						posViceHeadStr = "Deputy Head of the Division" + deptNameCode;
						break;
					case Hierarchy.Department:
						maxNumOfWorkersInDept = (maxNumOfWorkersInDept <= 3) ? 3 : r.Next(3, maxNumOfWorkersInDept);
						posHead = Positions.DeptDirector;
						posHeadStr = "Director" + deptNameCode;
						posViceHead = Positions.ViceDeptDirector;
						posViceHeadStr = "Vice Director" + deptNameCode;
						break;
				}

				int numOfEmployees = r.Next(1, maxNumOfWorkersInDept - 2 + 1);
				int numOfInterns = maxNumOfWorkersInDept - 2 - numOfEmployees;
				
				// Adding head of the organization/division/department
				orgToCreate.AddWorker(
					new Director("First_" + UniqueID.Name(),
								 "Last_" + UniqueID.Name(),
								 GenRandomDate(new DateTime(1950, 1,1), new DateTime(1981,12, 31)),
								 GenRandomDate(deptCD, DateTime.Now),
								 dept.DeptID,
								 posHeadStr,
								 posHead));
				// Adding vice head
				orgToCreate.AddWorker(
					new Employee("First_" + UniqueID.Name(),
								 "Last_" + UniqueID.Name(),
								 GenRandomDate(new DateTime(1950, 1, 1), new DateTime(1981, 12, 31)),
								 GenRandomDate(deptCD, DateTime.Now),
								 dept.DeptID,
								 posViceHeadStr,
								 posViceHead));
				// Adding employees
				for (int i = 1; i <= numOfEmployees; i++)
					orgToCreate.AddWorker(
						new Employee("First_" + UniqueID.Name(),
									 "Last_" + UniqueID.Name(),
									 GenRandomDate(new DateTime(1950, 1, 1), new DateTime(1996, 12, 31)),
									 GenRandomDate(deptCD, DateTime.Now),
									 dept.DeptID,
									 "Employee"));
				// Adding interns
				for (int i = 1; i <= numOfInterns; i++)
					orgToCreate.AddWorker(
						new Intern ("First_" + UniqueID.Name(),
									"Last_" + UniqueID.Name(),
									GenRandomDate(new DateTime(1990, 1, 1), new DateTime(2003, 12, 31)),
									GenRandomDate(deptCD, DateTime.Now),
									dept.DeptID,
									"Intern"));
			}

			DateTime GenRandomDate(DateTime sd, DateTime ed)
			{
				if (sd >= ed) return ed;
				DateTime ndate;
				int ny = r.Next(sd.Year, ed.Year + 1);
				int nm = r.Next(1, 13);
				int nd = 1;
				if (nm == 2) nd = r.Next(1, 29);
				else if (nm == 4 || nm == 6 || nm == 9 || nm == 11)
					nd = r.Next(1, 31);
				else nd = r.Next(1, 32);
				ndate = new DateTime(ny, nm, nd);
				if (ndate < sd) ndate = sd;
				if (ndate > ed) ndate = ed;
				return ndate;
			}

		}

	}
}
