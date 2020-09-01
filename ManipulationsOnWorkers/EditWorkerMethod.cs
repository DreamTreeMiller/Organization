using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.ManipulationsOnWorkers
{
	public class EditWorkerMethod
	{
		public void EditWorker(Organization org, Worker w, string[] newData)
		{
			if (w == null) return;
			BaseDepartment d = org.GetDepartment(w.DeptID);


		}


	}
}
