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

namespace MLM
{
	/// <summary>
	/// Interaction logic for EditWorker.xaml
	/// </summary>
	public partial class EditWorkerMenu : Window
	{
		public IWorkerDTO wCopy;
		int hoursWorked = 12;

		public EditWorkerMenu(IWorkerDTO w, string deptName, List<IPositionTuple> posList)
		{
			InitializeComponent();

			wCopy = (IWorkerDTO)w.Clone();
			EditWorkerGrid.DataContext = wCopy;
			DeptNameDisplay.Text				= deptName;
			PositionEntryBox.ItemsSource		= posList;
			PositionEntryBox.SelectedItem		= posList.Find(p => p.Pos == w.Position);

			// Showing salary differenty for different types of workers
			string salaryBasePrefix = "$";
			string salaryBaseSuffix = "";
			string hoursWorkedSuffix = "";

			if (w is IDirector)
				salaryBaseSuffix = " (15% from salaries of subordinates)";
			
			SalaryBaseEntryBox.Text = $"{w.Salary}";
			
			if (w is IEmployee)
			{
				salaryBasePrefix = $"${w.Salary} = $";
				SalaryBaseEntryBox.Text = $"{w.salaryBase}";
				salaryBaseSuffix = $" per hour, for ";
				hoursWorked = (w as IEmployee).HoursWorked;
				HoursWorkedEntryBox.Text = $"{hoursWorked}";
				hoursWorkedSuffix = " hours.";
			}

			salarybaseprefix.Text = salaryBasePrefix;
			salarybasesuffix.Text = salaryBaseSuffix;
			hoursworkedsuffix.Text = hoursWorkedSuffix;

		}

		private void btnOk_EditWorker_Click(object sender, RoutedEventArgs e)
		{
			//Check if new salary is not 
			int newValue = 0;
			bool result = Int32.TryParse(SalaryBaseEntryBox.Text, out newValue);
			
			// If no valid string was entered as number
			// Or salary is zero, don't change anything
			if (result && newValue != 0)
			{
				wCopy.salaryBase = newValue;
			}

			if (wCopy is IEmployee)
			{
				result = Int32.TryParse(SalaryBaseEntryBox.Text, out newValue);

				// If no valid string was entered as number
				// Or salary is zero, don't change anything
				if (result && newValue != 0)
				{
					wCopy.salaryBase = newValue;
				}
			}
			// If both name fields are empty - don't change anything
			if (!String.IsNullOrEmpty(FirstNameEntryBox.Text) ||
				!String.IsNullOrEmpty(LastNameEntryBox.Text))
			{
				wCopy.FirstName = FirstNameEntryBox.Text;
				wCopy.LastName = LastNameEntryBox.Text;
			}


			this.DialogResult = true;
		}

		private void PositionEntryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((PositionEntryBox.SelectedItem as IPositionTuple).Pos)
			{
				case Positions.Intern:
					salarybaseprefix.Text = "$";
					SalaryBaseEntryBox.Text = "500";
					salarybasesuffix.Text = " per month";
					HoursWorkedEntryBox.Visibility = Visibility.Collapsed;
					hoursworkedsuffix.Visibility = Visibility.Collapsed;
					break;
				case Positions.President:
				case Positions.DivisionHead:
				case Positions.DeptDirector:
					salarybaseprefix.Text = "$";
					SalaryBaseEntryBox.Text = $"{wCopy.Salary}";
					salarybasesuffix.Text = "15% of salaries of employees and subdepartments.\n" +
											"Minimum $1,300 per month";
					HoursWorkedEntryBox.Visibility = Visibility.Collapsed;
					hoursworkedsuffix.Visibility = Visibility.Collapsed;
					break;
				default:
					salarybaseprefix.Text = $"${wCopy.Salary} = $";
					SalaryBaseEntryBox.Text = $"{wCopy.salaryBase}";
					salarybasesuffix.Text = $" per hour, for ";

					HoursWorkedEntryBox.Visibility = Visibility.Visible;
					HoursWorkedEntryBox.Text = $"{hoursWorked}";

					hoursworkedsuffix.Visibility = Visibility.Visible;
					hoursworkedsuffix.Text = " hours.";
					break;
			}
		}
	}
}
