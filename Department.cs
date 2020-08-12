using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
	class Department
	{
		public string Name { get; set; }
		public Worker Director { get; set; }
		public Worker ViceDir { get; set; }
		public List<Department> SubDepts { get; set; }


	}
}
