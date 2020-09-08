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
		public void Serialize(string path, IOrganization org)
		{
			string json = JsonConvert.SerializeObject(org as Organization);
			System.IO.File.WriteAllText(path, json);
		}

		public IOrganization Deserialize(string path)
		{
			string json = System.IO.File.ReadAllText(path);
			Organization org = (Organization)JsonConvert.DeserializeObject(json);
			return org;
		}
	}
}
