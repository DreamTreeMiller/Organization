using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.InterfacesActions
{
	/// <summary>
	/// Interface for JSON de/serialization
	/// </summary>
	public interface I_Json
	{
		void Serialize(string path, IOrganization org);

		IOrganization Deserialize(string path);
	}
}
