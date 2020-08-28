using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class PositionsTuple
	{
		public Positions Pos { get; }
		public string PositionName { get; }

		public PositionsTuple(Positions pos, string pn)
		{
			Pos = pos;
			PositionName = pn;
		}
	}

	public class PositionsTable
	{
		/// <summary>
		/// List of position names 
		/// It is used in Add worker dialog window
		/// </summary>
		public List<PositionsTuple> PositionsNames = new List<PositionsTuple>()
		{
		new PositionsTuple(Positions.President,		   "President"),
		new PositionsTuple(Positions.VicePresident,    "VicePresident"),
		new PositionsTuple(Positions.DivisionHead,     "Head of the Division"),
		new PositionsTuple(Positions.ViceDivisionHead, "Deputy Head of the Division"),
		new PositionsTuple(Positions.DeptDirector,     "Director"),
		new PositionsTuple(Positions.ViceDeptDirector, "Vice Director"),
		new PositionsTuple(Positions.Employee,         "Employee"),
		new PositionsTuple(Positions.Intern,           "Intern")
		};

		public string this[Positions index]
		{
			get { return PositionsNames[(int)index].PositionName; }
		}

		public List<PositionsTuple> Available(BaseDepartment d)
		{
			// For each hierarchy level provide proper list of available positions
			// i.e. you can't add Head of the Division at the Department level
			var availablePositionsList = new List <PositionsTuple> (PositionsNames);

			// if d is HQ 
			if (d is HQ)
				// take off Division and Department leaders & vice leaders positions from the list
				availablePositionsList.RemoveRange(2, 4);

			// if d is Division 
			if (d is Division)
			{
				// take off President and Vice President from the list
				availablePositionsList.RemoveRange(0, 2);

				// and Department Director and Vice Director
				availablePositionsList.RemoveRange(2, 2);
			}
			// if d is Department
			if (d is Department)
				// take off President and Vice President, Head and Vice Head of Division from the list
				availablePositionsList.RemoveRange(0, 4);

			return availablePositionsList;
		}
	}
}
