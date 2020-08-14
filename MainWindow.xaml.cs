using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
		Organization Apple;
		public MainWindow()
		{
			InitializeComponent();
			Apple = new Organization("Apple Inc.");
			//unixmilliseconds.ItemsSource = ;
		}
	}
}

#region Experiments
		//DateTime start, end;
		//TimeSpan elapsed;
//Worker w = new Employee("John", "Smith", DateTime.Parse("01.01.1995"),
//  DateTime.Today, "BigDept", "HardWork", "Developer", 12);

//w.GetPaid(40);
//start = DateTime.Now;
//starttime.Text = $"{start}";
//for (int i = 0; i < 10_000_000; i++)
//{
//	guidlist.Add($"{Guid.NewGuid()}");
//	//do
//	//{
//	//	currID = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
//	//} while (currID == unixmsc.Last());

//}
//guidlist.Sort();
//end = DateTime.Now;
//string equals = "Equals were not found!!!";
//for (int i=1; i <1_000_000; i++)
//	if(guidlist[i-1] == guidlist[i])
//	{
//		equals = $"Equals are found {guidlist[i]}";
//		break;
//	}
//MessageBox.Show(equals);

//endtime.Text = $"{end}";
//elapsed = end - start;
//elapsedtime.Text = $"{elapsed}";
//this.Dispatcher.Invoke(() => MessageBox.Show($"Time start {start}\n" + 
//	                                         $"Time now {end}\n" + 
//											 $"Time elapsed {elapsed}"));
//for (int i = 0; i < 10; i++)
//{
//	Guid guidone = Guid.NewGuid();
//	byte[] guid = guidone.ToByteArray();
//	ulong ID = ((ulong)guid[3] << 8 * 5) |        // Сдвиг 
//		 ((ulong)guid[2] << 8 * 4) |
//		 ((ulong)guid[1] << 8 * 3) |
//		 ((ulong)guid[0] << 8 * 2) |
//		 ((ulong)guid[5] << 8 * 1) |
//		  (ulong)guid[4];
//	//string ID = "";
//	//for (int j = 0; j < 16; j++) ID += $"{guid[j]:x} ";
//	guidlist.Add($"{guidone}".Substring(0, 8) + $"{guidone}".Substring(9, 4));
//	//guidlist.Add(ID);
//	guidlist.Add($"{ID:x}");
//}
#endregion
