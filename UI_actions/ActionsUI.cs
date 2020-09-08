using MLM.InterfacesActions;
using MLM.Interfaces;

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
