using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	/// <summary>
	/// Generates unique numeric ID or strings
	/// </summary>
	public static class UniqueID
	{
		/// <summary>
		/// Generates pseudo unique 6 byte long numeric ID
		/// </summary>
		/// <returns>ulong pseudo unique ID</returns>
		public static ulong Generate()
		{
			// We take first 6 bytes of Guid and compose a unique ulong ID out of them
			byte[] guid = Guid.NewGuid().ToByteArray();
			return ((ulong)guid[3] << 8 * 5) |        // This is the oder of bytes 
					((ulong)guid[2] << 8 * 4) |        // according to the string format of Guid
					((ulong)guid[1] << 8 * 3) |
					((ulong)guid[0] << 8 * 2) |
					((ulong)guid[5] << 8 * 1) |
					 (ulong)guid[4];
		}

		/// <summary>
		/// Generates pseudo unique string to add to names, last names
		/// </summary>
		/// <returns>Pseudo unique string 5 char long</returns>
		public static string Name()
		{
			return Guid.NewGuid().ToString().Substring(0, 5);
		}
	}

	class CompareByPosition : IComparer<Worker>
	{
		public int Compare(Worker x, Worker y)
		{
			//if (x == null && y == null) return 0;           // Сотрудники идентичны
			//if (x == null) return -1;                       // x - null, y not null, y is greater
			//if (y == null) return 1;                       // x is not null, y is null, x is greater
			return x.Position.CompareTo(y.Position);
		}

	}

	/// <summary>
	/// Компаратор для поля зарплата
	/// </summary>
	class CompareBySalary : IComparer<Employee>
	{
		public int Compare(Employee x, Employee y)
		{
			if (x == null && y == null) return 0;           // Сотрудники идентичны
			if (x == null) return -1;                       // x - null, y not null, y is greater
			if (y == null) return 1;                       // x is not null, y is null, x is greater
			return x.Salary.CompareTo(y.Salary);
		}
	}


}


