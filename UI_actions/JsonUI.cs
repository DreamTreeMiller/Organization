using MLM.Interfaces;
using MLM.ActionsBackEnd;
using MLM.InterfacesActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.ActionsUserInterface
{
	public class JsonUI
	{
		/// <summary>
		/// Back-end methods
		/// </summary>
		private readonly I_Json JsonBE;

		public JsonUI()
		{
			JsonBE = new JsonSerializer();
		}

		/// <summary>
		/// Opens dialog to specify JSON file name where organization will be serialized,
		/// and serializes organization
		/// </summary>
		/// <param name="org">Organization to serialize</param>
		public void Serialize(IOrganization org)
		{
			IRetrieve Get = org as IRetrieve;
			// Configure save file dialog box
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = Get.RootDepartment().DeptName; // Default file name
			dlg.DefaultExt = ".json"; // Default file extension
			dlg.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

			// Show save file dialog box
			bool? result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result != true) return;

			// Save document
			JsonBE.Serialize(dlg.FileName, org);
		}

		/// <summary>
		/// Opens dialog to select JSON file with organization.
		/// Deserializes file into current organization.
		/// </summary>
		/// <returns></returns>
		public IOrganization Deserialize()
		{
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.FileName = "CompanyName"; // Default file name
			dlg.DefaultExt = ".json"; // Default file extension
			dlg.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

			// Show open file dialog box
			bool? result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result != true) return null;

			// Open document
			return JsonBE.Deserialize(dlg.FileName);
		}
	}
}
