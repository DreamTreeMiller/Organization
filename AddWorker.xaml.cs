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

		public AddWorker(List<DeptSimple> DeptList, List<PositionsTuple> PosList)
		{
			InitializeComponent();

			//DeptSimple deptSimple = new DeptSimple();
			//DeparmmentEntryBox.DataContext = deptSimple;
			DeparmmentEntryBox.ItemsSource = DeptList;

			//PositionsTuple positionsTuple = new PositionsTuple();
			//PositionEntryBox.DataContext = positionsTuple;
			PositionEntryBox.ItemsSource = PosList; // WorkersTable.PositionsNames;

		}

		private void btnOk_AddEmployee_Click(object sender, RoutedEventArgs e)
		{
			if (DateOfBirthPicker.SelectedDate == null ||
				(String.IsNullOrEmpty(FirstNameEntryBox.Text) && 
				 String.IsNullOrEmpty(LastNameEntryBox.Text)) ||
				DeparmmentEntryBox.SelectedItem == null ||
				PositionEntryBox.SelectedItem == null)
				this.DialogResult = false;
			else 
				this.DialogResult = true;
		}
	}
}
