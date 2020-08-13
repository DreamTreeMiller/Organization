using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	enum Positions
	{
		// Managers level
		President		 = 1,
		VicePresident	 = 2,
		DivisionHead	 = 3,
		DeptDirector	 = 4,
		// Employee level
		ViceDivisionHead = 5,
		ViceDeptDirector = 6,
		Employee		 = 7,
		// Intern
		Intern			 = 8
	}

	enum PaymentType
	{
		Standard,
		Random
	}


}
