using MLM.Interfaces;
using MLM.ActionsBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Organizaton;

namespace MLM
{
	struct PositionTuple : IPositionTuple
	{
		/// <summary>
		/// Position type
		/// </summary>
		public Positions Pos { get; }

		/// <summary>
		/// Name of position
		/// </summary>
		public string PositionName { get; }

		public PositionTuple(Positions p, string n)
		{
			Pos = p;
			PositionName = n;
		}
	}

	/// <summary>
	/// Keeps list of all positions in the organization.
	/// Provides list of positions available in different departments and situations
	/// </summary>
	public class PositionsTable : IPositions
	{
		/// <summary>
		/// List of position names 
		/// It is used in Add worker dialog window
		/// </summary>
		public List<IPositionTuple> PositionsDict = new List<IPositionTuple>()
		{
			new PositionTuple(Positions.President,        "President"),
			new PositionTuple(Positions.VicePresident,    "Vice President"),
			new PositionTuple(Positions.DivisionHead,     "Head of the Division"),
			new PositionTuple(Positions.ViceDivisionHead, "Deputy Head of the Division"),
			new PositionTuple(Positions.DeptDirector,     "Director"),
			new PositionTuple(Positions.ViceDeptDirector, "Vice Director"),
			new PositionTuple(Positions.Employee,         "Employee"),
			new PositionTuple(Positions.Intern,           "Intern")
		};

		/// <summary>
		/// Provides access to the positions list via value of Positions enum type
		/// </summary>
		/// <param name="index">index of Positions enum type</param>
		/// <returns></returns>
		public string this[Positions index]
		{
			get { return PositionsDict[(int)index].PositionName; }
		}

		private Organization org;

		public PositionsTable(Organization org)
		{
			this.org = org;
		}
		/// <summary>
		/// Provides list of positions available to take by an employee
		/// </summary>
		/// <param name="org">
		/// Organization, it's necessary in order to check 
		/// if specified department has director or not
		/// </param>
		/// <param name="d">Department</param>
		/// <returns></returns>
		public List<IPositionTuple> Available(BaseDepartment d, bool keepDirector)
		{
			// For each hierarchy level provide proper list of available positions
			// i.e. you can't add Head of the Division at the Department level
			var availablePositionsList = new List<IPositionTuple>(PositionsDict);

			if (d is HQ)
				// take off Division and Department leaders & vice leaders positions from the list
				availablePositionsList.RemoveRange(2, 4);

			if (d is Division)
			{
				// take off President and Vice President from the list
				availablePositionsList.RemoveRange(0, 2);

				// and Department Director and Vice Director
				availablePositionsList.RemoveRange(2,2);
			}
			// if d is Department
			if (d is Department)
				// take off President and Vice President, Head and Vice Head of Division from the list
				availablePositionsList.RemoveRange(0, 4);

			// also you can't appoint someone at the leader's position if leader is present in the dept
			// In case of adding new worker or editing non-director while director is present
			// remove boss position from the list 
			if ((org.Director(d) != null) && !keepDirector)
				availablePositionsList.RemoveRange(0, 1);

			return availablePositionsList;
		}
	}
}
