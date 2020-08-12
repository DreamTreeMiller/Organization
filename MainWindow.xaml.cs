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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MLM
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Worker w = new Employee("John", "Smith", DateTime.Parse("01.01.1995"),
			  DateTime.Today, "BigDept", "HardWork", "Developer", 12);

			w.GetPaid(40);
			MessageBox.Show($"Employee salary{w.Salary}");
		}
	}
}
