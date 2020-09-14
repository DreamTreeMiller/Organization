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

namespace MLM.ManipulationsOnWorkers
{
	/// <summary>
	/// Interaction logic for AddWorker.xaml
	/// </summary>
	public partial class AddWorkerMenu : Window
	{

		public AddWorkerMenu(string DeptName, List<IPositionTuple> PosList)
		{
			InitializeComponent();

			DeptNameDisplay.Text = DeptName;
			
			// List of available positions 
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
			switch ((PositionEntryBox.SelectedItem as IPositionTuple).Pos)
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

		/// <summary>
		/// Added this warning 
		/// </summary>
		/// <param name="warning"></param>
		public void AddingMistakeWarning(int warning)
		{
			if (warning == -1) MessageBox.Show("Worker with same ID already exists!");
			if (warning == -2) MessageBox.Show( "You are trying to add a director\n" +
												"while department already has one!");
		}
	}
}
