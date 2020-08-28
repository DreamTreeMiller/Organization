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
	/// Interaction logic for EditWorker.xaml
	/// </summary>
	public partial class EditWorker : Window
	{
		BaseDepartment currDept;
		Worker currWorker;
		public EditWorker(Worker w, BaseDepartment d, List<PositionsTuple> PosList)
		{
			InitializeComponent();
			currWorker = w;
			currDept = d;
			FirstNameEntryBox.Text				= w.FirstName;
			LastNameEntryBox.Text				= w.LastName;
			DateOfBirthPicker.SelectedDate		= w.DateOfBirth;
			DeptNameDisplay.Text				= d.DeptName;
			EmployedOnDatePicker.SelectedDate	= w.EmployedOn;
			PositionEntryBox.ItemsSource		= PosList;
			PositionEntryBox.SelectedItem		= PosList.Find(p => p.Pos == w.Position);
			string salaryBasePrefix = "$";
			string salaryBaseSuffix = "";
			if (w is Director)
				salaryBaseSuffix = " (15% from salaries of subordinates)";
			SalaryBaseEntryBox.Text = $"{w.Salary}";
			if (w is Employee)
			{
				salaryBasePrefix = $"${w.Salary}";
				SalaryBaseEntryBox.Text = $"${w.salaryBase}";
				salaryBaseSuffix = $" per hour, for {(w as Employee).HoursWorked} hours.";
			}
			salarybaseprefix.Text = salaryBasePrefix;
			salarybasesuffix.Text = salaryBaseSuffix;

		}

		private void btnOk_EditWorker_Click(object sender, RoutedEventArgs e)
		{
			var newSalaryBase = 0;
			bool result = Int32.TryParse(SalaryBaseEntryBox.Text, out newSalaryBase);
			if (!result) return;

			if ((String.IsNullOrEmpty(FirstNameEntryBox.Text) &&
				  String.IsNullOrEmpty(LastNameEntryBox.Text)) ||
				  newSalaryBase == 0
				)
				this.DialogResult = false;
			else
				this.DialogResult = true;
		}

		private void PositionEntryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((PositionEntryBox.SelectedItem as PositionsTuple).Pos)
			{
				case Positions.Intern:
					SalaryBaseEntryBox.Text = "$500 per month";
					break;
				case Positions.President:
				case Positions.DivisionHead:
				case Positions.DeptDirector:
					SalaryBaseEntryBox.Text = "15% of salaries of employees and subdepartments.\n" +
											"Minimum $1,300 per month";
					break;
				default:
					SalaryBaseEntryBox.Text = "$12 per hour";
					break;
			}
		}
	}
}
