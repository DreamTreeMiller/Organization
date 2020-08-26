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

namespace MLM
{
	/// <summary>
	/// Interaction logic for MoveWorker.xaml
	/// </summary>
	public partial class MoveWorker : Window
	{
		/// <summary>
		/// Dialog window to move a worker from current department to another
		/// </summary>
		/// <param name="w">Being moved worker</param>
		/// <param name="currDept">Department where worker currently works in</param>
		/// <param name="deptTable">List of available destination departments</param>
		public MoveWorker(Worker w, string currDept, List<BaseDepartment> deptTable)
		{
			InitializeComponent();

			// Show name of the being moved worker
			FirstNameDisplay.Text = w.FirstName;
			LastNameDisplay.Text  = w.LastName;

			CurrentDepartmentDisplay.Text = currDept;
			DeparmmentEntryBox.ItemsSource = deptTable;
		}

		/// <summary>
		/// Processes Ok button or Enter key click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_MoveWorker_Click(object sender, RoutedEventArgs e)
		{
			// If destination department was not selected don't do anything
			if (DeparmmentEntryBox.SelectedItem == null)
				this.DialogResult = false;
			else
				this.DialogResult = true;
		}
	}
}
