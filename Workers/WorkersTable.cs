using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	public class WorkersTable
	{
		private class CompareByPosition : IComparer<Worker>
		{
			public int Compare(Worker x, Worker y)
			{
				if (x == null && y == null) return 0;           // Сотрудники идентичны
				if (x == null) return -1;                       // x - null, y not null, y is greater
				if (y == null) return 1;                       // x is not null, y is null, x is greater
				return x.Position.CompareTo(y.Position);
			}
		}

		/// <summary>
		/// Компаратор для поля зарплата
		/// </summary>
		private class CompareBySalary : IComparer<Worker>
		{
			public int Compare(Worker x, Worker y)
			{
				if (x == null && y == null) return 0;           // Сотрудники идентичны
				if (x == null) return -1;                       // x - null, y not null, y is greater
				if (y == null) return 1;                       // x is not null, y is null, x is greater
				return x.Salary.CompareTo(y.Salary);
			}
		}
	}
}
