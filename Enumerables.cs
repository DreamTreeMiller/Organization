using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public enum Positions
	{
		President,
		VicePresident,
		DivisionHead,
		ViceDivisionHead,
		DeptDirector,
		ViceDeptDirector,
		Employee,
		Intern
	}

	public class PositionsTuple
	{
		public Positions Pos { get; set; }
		public string PositionName { get; set; }
	}

	public enum PaymentType
	{
		Standard,
		Random
	}

	public enum Hierarchy
	{
		Top, Division, Department
	}

}
