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
	/// Interaction logic for DeleteDepartmentMenu.xaml
	/// </summary>
	public partial class DeleteDepartmentMenu : Window
	{
		// true - self exclude way of deletion (moving staff and sub depts)
		// false - complete removal of all employees and subdepartments
		public bool selfExcludeOrCompletely = true;
		public DeleteDepartmentMenu(IDepartmentDTO d)
		{
			InitializeComponent();
			deptnameid.Text = $"ID: {d.DeptID},  {d.DeptName}";
		}

		private void btnOk_DeleteDepartment_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void SelfExcludeDeletion_Checked(object sender, RoutedEventArgs e)
		{
			selfExcludeOrCompletely = true;
		}

		private void CompleteDeletion_Checked(object sender, RoutedEventArgs e)
		{
			selfExcludeOrCompletely = false;
		}
	}
}
