using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.InterfacesActions
{
	public interface ICreateOrganization
	{
		/// <summary>
		/// Generates ones whole organization with random workers and subdepts
		/// </summary>
		/// <param name="maxDepth">1 - no sub departments, 2 - top and 1 level of departments, etc.</param>
		/// <param name="maxSubDepts">Maximum number of subdepartments on next level</param>
		/// <param name="deptNameCode">Code to add to department name "4", "53", "2117", etc. Top most is "1".
		/// First call of the method with "" code (empty string)</param>
		/// <param name="maxNumOfWorkersInDept">Maximum number of workers in the current deparment</param>
		/// <returns>Created organization via IOrganization interface</returns>
		IOrganization Organization(int maximumDepth,
								   int maximumSubDepts,
								   string orgName,
								   int maximumNumOfWorkersInDept);
	}
}
