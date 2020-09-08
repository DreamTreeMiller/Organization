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
		/// <summary>
		/// Seralizes the organization in JSON file
		/// </summary>
		/// <param name="path">Path to save organization info in JSON format</param>
		/// <param name="org">Organization to save</param>
		void Serialize(string path, IOrganization org);

		/// <summary>
		/// Retrieves organization from JSON file
		/// </summary>
		/// <param name="path">Path to file of organization in JSON format</param>
		/// <returns>Organization</returns>
		IOrganization Deserialize(string path);
	}
}
