using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	/// <summary>
	/// CompareTo method treads enumerables as ints
	/// in Positions type 0 - President is the highest position and 7 - intern - lowest
	/// but here is opposite. Therefore I changed sign
	/// </summary>
	public class ByPositionAscending : IComparer<IWorker>
	{
		public int Compare(IWorker ix, IWorker iy)
		{
			var x = ix as Worker;
			var y = iy as Worker;
			if (x == null && y == null) return 0;           // Сотрудники идентичны
			if (x == null) return 1;                        // x - null, y not null, y is greater
			if (y == null) return -1;                       // x is not null, y is null, x is greater
			return -x.Position.CompareTo(y.Position);
		}
	}

	/// <summary>
	/// CompareTo method treads enumerables as ints
	/// in Positions type 0 - President is the highest position and 7 - intern - lowest
	/// but here is opposite. Therefore I changed sign
	/// </summary>
	public class ByPositionDescending : IComparer<IWorker>
	{
		public int Compare(IWorker ix, IWorker iy)
		{
			var x = ix as Worker;
			var y = iy as Worker;
			if (x == null && y == null) return 0;           // Сотрудники идентичны
			if (x == null) return -1;                        // x - null, y not null, y is greater
			if (y == null) return 1;                       // x is not null, y is null, x is greater
			return x.Position.CompareTo(y.Position);
		}
	}
}
