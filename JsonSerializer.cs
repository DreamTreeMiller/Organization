using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace MLM
{
	static class JsonSerializer
	{
		public static void Serialize (string path, Organization org)
		{
			string json = JsonConvert.SerializeObject(org);
			System.IO.File.WriteAllText(path, json);
		}

		public static Organization Deserialize (string path)
		{
			Organization org;
			string json = System.IO.File.ReadAllText(path);
			org = (Organization)JsonConvert.DeserializeObject(json);
			return org;
		}
	}
}
