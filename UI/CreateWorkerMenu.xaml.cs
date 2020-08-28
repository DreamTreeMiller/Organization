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
	/// Interaction logic for AddWorker.xaml
	/// </summary>
	public partial class AddWorker : Window
	{

		public AddWorker(string DeptName, List<PositionsTuple> PosList)
		{
			InitializeComponent();

			DeptNameDisplay.Text = DeptName;
			// List of available positions - WorkersTable.PositionsNames
			PositionEntryBox.ItemsSource  = PosList;  

			// Set lowest position as default
			PositionEntryBox.SelectedItem = PosList.Find(p => p.Pos == Positions.Intern);
		}

		private void btnOk_AddEmployee_Click(object sender, RoutedEventArgs e)
		{
			if (DateOfBirthPicker.SelectedDate == null ||
				(String.IsNullOrEmpty(FirstNameEntryBox.Text) && 
				 String.IsNullOrEmpty(LastNameEntryBox.Text)) ||
				PositionEntryBox.SelectedItem == null)
				this.DialogResult = false;
			else 
				this.DialogResult = true;
		}

		private void PositionEntryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((PositionEntryBox.SelectedItem as PositionsTuple).Pos)
			{
				case Positions.Intern:
					SalaryBaseResult.Text = "$500 per month";
					break;
				case Positions.President:
				case Positions.DivisionHead:
				case Positions.DeptDirector:
					SalaryBaseResult.Text = "15% of salaries of employees and subdepartments.\n" +
											"Minimum $1,300 per month";
					break;
				default:
					SalaryBaseResult.Text = "$12 per hour";
					break;
			}
		}

	}
}
