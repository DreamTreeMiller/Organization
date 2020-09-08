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
		IOrganization Organization(int maximumDepth,
								   int maximumSubDepts,
								   string orgName,
								   int maximumNumOfWorkersInDept);
	}
}
