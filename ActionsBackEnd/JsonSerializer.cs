using MLM.Interfaces;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MLM.Organizaton;

namespace MLM.ActionsBackEnd
{
	public class JsonSerializer : I_Json
	{
		/// <summary>
		/// Seralizes the organization in JSON file
		/// </summary>
		/// <param name="path">Path to save organization info in JSON format</param>
		/// <param name="org">Organization to save</param>
		public void Serialize(string path, IOrganization org)
		{
			string json = JsonConvert.SerializeObject(org as Organization);
			System.IO.File.WriteAllText(path, json);
		}

		/// <summary>
		/// Retrieves organization from JSON file
		/// </summary>
		/// <param name="path">Path to file of organization in JSON format</param>
		/// <returns>Organization</returns>
		public IOrganization Deserialize(string path)
		{
			string json = System.IO.File.ReadAllText(path);
			Organization org = (Organization)JsonConvert.DeserializeObject(json);
			return org;
		}
	}
}
