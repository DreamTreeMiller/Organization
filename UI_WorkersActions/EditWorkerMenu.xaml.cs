using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MLM
{
	/// <summary>
	/// Interaction logic for EditWorker.xaml
	/// </summary>
	public partial class EditWorkerMenu : Window
	{
		public EditWorkerValidation wCopy;

		public EditWorkerMenu(IWorker w,
							  IDepartmentDTO d,
							  bool hasDirector,
							  List<IPositionTuple> posList,
							  DateTime orgCreationDate)
		{
			InitializeComponent();
			wCopy = new EditWorkerValidation(w, d, hasDirector, orgCreationDate);
			EditWorkerGrid.DataContext = wCopy;

			DateOfBirthPicker.SelectedDate = wCopy.DateOfBirth;
			EmployedOnDatePicker.SelectedDate = wCopy.EmployedOn; ;
			DeptNameDisplay.Text = d.DeptName;
			PositionEntryBox.ItemsSource = posList;
			PositionEntryBox.SelectedItem = posList.Find(p => p.Pos == w.Position);


			DirectorSalary.Visibility = Visibility.Collapsed;
			EmployeeSalary.Visibility = Visibility.Collapsed;
			InternSalary.Visibility   = Visibility.Collapsed;

			if (w is IDirector)
				DirectorSalary.Visibility = Visibility.Visible;

			if (w is IEmployee)
				EmployeeSalary.Visibility = Visibility.Visible;

			if (w is IIntern)
				InternSalary.Visibility   = Visibility.Visible;
		}


		private void btnOk_EditWorker_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void PositionEntryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// First set all salary showing bar to invisible
			DirectorSalary.Visibility = Visibility.Collapsed;
			EmployeeSalary.Visibility = Visibility.Collapsed;
			InternSalary.Visibility = Visibility.Collapsed;

			wCopy.Position = (PositionEntryBox.SelectedItem as IPositionTuple).Pos;
			wCopy.PositionTitle = (PositionEntryBox.SelectedItem as IPositionTuple).PositionName;

			switch ((PositionEntryBox.SelectedItem as IPositionTuple).Pos)
			{
				case Positions.Intern:
					InternSalary.Visibility = Visibility.Visible;
					(wCopy as IWorkerDTO).SelectedWorkerType = WorkerHierarchy.Intern;
					break;

				case Positions.President:
				case Positions.DivisionHead:
				case Positions.DeptDirector:
					DirectorSalary.Visibility = Visibility.Visible;
					(wCopy as IWorkerDTO).SelectedWorkerType = WorkerHierarchy.Director;
					break;

				default:
					EmployeeSalary.Visibility = Visibility.Visible;
					(wCopy as IWorkerDTO).SelectedWorkerType = WorkerHierarchy.Employee;
					break;
			}
		}

		/// <summary>
		/// Checks correctness of entered value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HourlyRateEntryBox_LostFocus(object sender, RoutedEventArgs e)
		{
			int value = 0;
			if (Int32.TryParse(HourlyRateEntryBox.Text, out value))
			{
				if (value < 1) value = 1;
				wCopy.HourlyRate = value;
			}
			else
				HourlyRateEntryBox.Text = $"{wCopy.HourlyRate}";

		}

		/// <summary>
		/// Checks correctness of entered value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HoursWorkedEntryBox_LostFocus(object sender, RoutedEventArgs e)
		{
			int value = 0;
			if (Int32.TryParse(HoursWorkedEntryBox.Text, out value))
			{
				if (value < 0) value = 0;
				wCopy.HoursWorked = value;
			}
			else
				HoursWorkedEntryBox.Text = $"{wCopy.HoursWorked}";
		}

		/// <summary>
		/// Checks correctness of entered value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InternSalaryEntryBox_LostFocus(object sender, RoutedEventArgs e)
		{
			int value = 0;
			if (Int32.TryParse(InternSalaryEntryBox.Text, out value))
			{
				if (value < 0)
					value = 0;
				else if (value > 1000)
					value = 1000;

				wCopy.IntSalary = value;
			}
			else
				InternSalaryEntryBox.Text = $"{wCopy.IntSalary}";
		}

		/// <summary>
		/// Checks if both first and last names fields are not empty
		/// Returns previous content of the FirstName TextBox and 
		/// focus back to it if both are empty 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FirstNameEntryBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(FirstNameEntryBox.Text)
				&&
				String.IsNullOrEmpty(LastNameEntryBox.Text))
			{
				FirstNameEntryBox.Text = wCopy.FirstName;
				MessageBox.Show("Both first and last names cannot be empty!");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					FirstNameEntryBox.Focus();
					FirstNameEntryBox.SelectionStart = FirstNameEntryBox.Text.Length;
				});
			}
		}

		/// <summary>
		/// Checks if both first and last names fields are not empty
		/// Returns previous content of the LastName TextBox and 
		/// focus back to it if both are empty 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LastNameEntryBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(FirstNameEntryBox.Text)
				&&
				String.IsNullOrEmpty(LastNameEntryBox.Text))
			{
				LastNameEntryBox.Text = wCopy.LastName;
				MessageBox.Show("Both first and last names cannot be empty!");
				Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					LastNameEntryBox.Focus();
					LastNameEntryBox.SelectionStart = LastNameEntryBox.Text.Length;
				});
			}
		}
	}
}
