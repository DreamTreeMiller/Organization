using MLM.InterfacesActions;
using MLM.Interfaces;
using MLM.ActionsBackEnd;
using MLM.ManipulationsOnWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Organizaton;

namespace MLM.ActionsUserInterface
{
	/// <summary>
	/// Accumulates all UI classes and methods 
	/// </summary>
	public class ActionsUI
	{
		public IRetrieve			Get;
		public IWorkersActions		Workers;
		public IDepartmentsActions	Departments;
		public JsonUI				Json;

		public ActionsUI(IOrganization org)
		{
			Get			= org as IRetrieve;
			Workers		= org as IWorkersActions;
			Departments = org as IDepartmentsActions;
			Json		= new JsonUI();
		}
	}
}
