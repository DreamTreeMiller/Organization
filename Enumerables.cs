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

	enum Hierarchy
	{
		Top, Division, Department
	}

	public static class UniqueID
	{
		public static ulong Generate()
		{
			// We take first 6 bytes of Guid and compose a unique ulong ID out of them
			byte[] guid = Guid.NewGuid().ToByteArray();
			return	((ulong)guid[3] << 8 * 5) |        // This is the oder of bytes 
					((ulong)guid[2] << 8 * 4) |        // according to the string format of Guid
					((ulong)guid[1] << 8 * 3) |
					((ulong)guid[0] << 8 * 2) |
					((ulong)guid[5] << 8 * 1) |
					 (ulong)guid[4];
		}

		public static string Name()
		{
			return Guid.NewGuid().ToString().Substring(0, 5);
		}
	}
}
