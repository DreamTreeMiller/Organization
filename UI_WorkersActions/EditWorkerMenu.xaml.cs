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
		public IWorkerDTO wCopy;
		WorkerHierarchy originalWorkerType;
		WorkerHierarchy selectedWorkerType;
		DateTime orgCreationDate;
		/// <summary>
		/// Class to correctly enter and amend worker's salary
		/// </summary>
		class DatesAndSalary : INotifyPropertyChanged
		{
			private DateTime _dateofbirth;
			public DateTime DateOfBirth
			{
				get => _dateofbirth;
				set
				{
					if (DateTime.Now.Subtract(value).Days / 365.25 < 18)
					{
						MessageBox.Show("Person should have been born at least 18 years ago!");
						return;
					}

					if (DateTime.Now.Subtract(value).Days / 365.25 > 70)
					{
						MessageBox.Show("Person elder than 70 cannot work in the company!");
						return;
					}
					_dateofbirth = value;
				}
			}

			private readonly DateTime orgCreationDate;
			private DateTime _employedon;
			public DateTime EmployedOn
			{
				get => _employedon;
				set
				{
					if (value > DateTime.Now)
					{
						MessageBox.Show("Employment date should be today or earlier.");
						return;
					}

					if (value < orgCreationDate)
					{
						MessageBox.Show("Person cannot be hired before organiation was created.");
						return;
					}
					_employedon = value;
				}
			}
			private int _dirsalary = 1300;
			public int DirSalary
			{
				get => _dirsalary;
				set
				{
					if (value < 1300) value = 1300;
					_dirsalary = value;
				}
			}
			public int EmpSalary
			{
				get { return HourlyRate * HoursWorked; }
				set { }
			}

			private int _hourlyrate = 12;
			public int HourlyRate
			{
				get => _hourlyrate;
				set
				{
					if (value < 1)
						_hourlyrate = 1;
					else
						_hourlyrate = value;
					NotifyPropertyChanged("EmpSalary");
				}
			}

			private int _hoursworked = 22 * 8;
			public int HoursWorked
			{
				get => _hoursworked;
				set
				{
					if (value < 0)
						_hoursworked = 0;
					else if (value > 744)
						_hoursworked = 744;
					else
						_hoursworked = value;
					NotifyPropertyChanged("EmpSalary");
				}
			}

			private int _intsalary = 500;

			public int IntSalary
			{
				get => _intsalary;
				set
				{
					if (value < 0)
						_intsalary = 0;
					else if (value > 1000)
						_intsalary = 1000;
					else
						_intsalary = value;
				}
			}

			public DatesAndSalary(DateTime orgCreationDate)
			{
				this.orgCreationDate = orgCreationDate;
			}

			public event PropertyChangedEventHandler PropertyChanged;

			// This method is called by the Set accessor of each property.  
			// The CallerMemberName attribute that is applied to the optional propertyName  
			// parameter causes the property name of the caller to be substituted as an argument.  
			private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		DatesAndSalary wSalary;

		public EditWorkerMenu(IWorkerDTO w,
							  IDepartmentDTO d,
							  bool hasDirector,
							  List<IPositionTuple> posList,
							  DateTime orgCreationDate)
		{
			InitializeComponent();

			wCopy = (IWorkerDTO)w.Clone();
			EditWorkerGrid.DataContext = wCopy;

			DeptNameDisplay.Text = d.DeptName;
			PositionEntryBox.ItemsSource = posList;
			PositionEntryBox.SelectedItem = posList.Find(p => p.Pos == w.Position);

			#region Initializing wSalary and StackPanels for correctly changing salary

			this.orgCreationDate = orgCreationDate;
			wSalary = new DatesAndSalary(orgCreationDate);

			wSalary.DateOfBirth = wCopy.DateOfBirth;
			DateOfBirthPicker.DataContext = wSalary;

			wSalary.EmployedOn = wCopy.EmployedOn;
			EmployedOnDatePicker.DataContext = wSalary;

			DirectorSalary.DataContext = wSalary;
			EmployeeSalary.DataContext = wSalary;
			InternSalary.DataContext = wSalary;

			DirectorSalary.Visibility = Visibility.Collapsed;
			EmployeeSalary.Visibility = Visibility.Collapsed;
			InternSalary.Visibility = Visibility.Collapsed;

			if (wCopy is IDirector)
			{
				originalWorkerType = WorkerHierarchy.Director;
				DirectorSalary.Visibility = Visibility.Visible;
				wSalary.DirSalary = wCopy.Salary;
			}

			// Calculating director's salary to show in the form
			// This is necessary if department currently does not have director
			// and we need to show salary of a worker if he will become a director
			if (hasDirector == false)
			{
				wSalary.DirSalary =
					(d.TotalDeptStaff_withoutBoss_Salary +
					 d.TotalSubDepartmentsSalary -
					 wCopy.Salary) / 100 * 15;
			}

			if (wCopy is IEmployee)
			{
				originalWorkerType = WorkerHierarchy.Employee;
				EmployeeSalary.Visibility = Visibility.Visible;

				wSalary.HourlyRate = (wCopy as IEmployee).HourlyRate;
				wSalary.HoursWorked = (wCopy as IEmployee).HoursWorked;
			}

			if (wCopy is IIntern)
			{
				originalWorkerType = WorkerHierarchy.Intern;
				InternSalary.Visibility = Visibility.Visible;
				wSalary.IntSalary = wCopy.Salary;
			}

			selectedWorkerType = originalWorkerType;
			#endregion

		}


		private void btnOk_EditWorker_Click(object sender, RoutedEventArgs e)
		{
			// At this point wCopy and wSalary has all updated valid values
			wCopy.DateOfBirth = wSalary.DateOfBirth;
			wCopy.EmployedOn  = wSalary.EmployedOn;

			IWorkerDTO newW = wCopy;
			// Now need to update salary fields based on newly selected position
			if (originalWorkerType != selectedWorkerType)
			{
				switch(selectedWorkerType)
				{
					case WorkerHierarchy.Director:
						newW = (IDirector)wCopy.Clone();
						newW.Salary = wCopy.Salary;				// actually this is not necessary because salary will be recalculated anyway
						break;
					case WorkerHierarchy.Employee:
						IEmployee newE = (IEmployee)wCopy.Clone();
						newE.HourlyRate = wSalary.HourlyRate;
						newE.HoursWorked = wSalary.HoursWorked;
						break;
					case WorkerHierarchy.Intern:
						newW = (IIntern)wCopy.Clone();
						newW.Salary = wCopy.Salary;
						break;
				}
			}

			wCopy = newW;
			this.DialogResult = true;
		}

		private void PositionEntryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// First set all salary showing bar to invisible
			DirectorSalary.Visibility = Visibility.Collapsed;
			EmployeeSalary.Visibility = Visibility.Collapsed;
			InternSalary.Visibility = Visibility.Collapsed;

			switch ((PositionEntryBox.SelectedItem as IPositionTuple).Pos)
			{
				case Positions.Intern:
					InternSalary.Visibility = Visibility.Visible;
					selectedWorkerType = WorkerHierarchy.Intern;
					break;

				case Positions.President:
				case Positions.DivisionHead:
				case Positions.DeptDirector:
					DirectorSalary.Visibility = Visibility.Visible;
					selectedWorkerType = WorkerHierarchy.Director;
					break;

				default:
					EmployeeSalary.Visibility = Visibility.Visible;
					selectedWorkerType = WorkerHierarchy.Employee;
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
				wSalary.HourlyRate = value;
			}
			else
				HourlyRateEntryBox.Text = $"{wSalary.HourlyRate}";

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
				wSalary.HoursWorked = value;
			}
			else
				HoursWorkedEntryBox.Text = $"{wSalary.HoursWorked}";
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

				wSalary.IntSalary = value;
			}
			else
				InternSalaryEntryBox.Text = $"{wSalary.IntSalary}";
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
