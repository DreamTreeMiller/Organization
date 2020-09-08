using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.InterfacesActions
{
	public interface IRetrieve
	{
		IDepartmentDTO RootDepartment();

		IWorkerDTO Director(IDepartmentDTO d);

		List<IWorkerDTO> OneDepartmentWorkersList(IDepartmentDTO d);

		List<IDepartmentDTO> DepartmentsList();

		List<IDepartmentDTO> SubDepartments(IDepartmentDTO d);
	}
}
