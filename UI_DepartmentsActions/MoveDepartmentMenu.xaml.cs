using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MLM.UI_DepartmentsActions
{
	/// <summary>
	/// Interaction logic for MoveDepartmentMenu.xaml
	/// </summary>
	public partial class MoveDepartmentMenu : Window
	{
		public IDepartmentDTO destDept;
		public MoveDepartmentMenu(IDepartmentDTO d, List<IDepartmentDTO> deptList)
		{
			InitializeComponent();
			IdDisplay.Text					= $"{d.DeptID}";
			DeptNameDisplay.Text			= d.DeptName;
			DeparmmentEntryBox.ItemsSource	= deptList;
		}

		private void btnOk_MoveDepartment_Click(object sender, RoutedEventArgs e)
		{
			// If destination department was not selected don't do anything
			if (DeparmmentEntryBox.SelectedItem == null)
				this.DialogResult = false;
			else
			{
				destDept = DeparmmentEntryBox.SelectedItem as IDepartmentDTO;
				this.DialogResult = true;
			}
		}
	}
}
