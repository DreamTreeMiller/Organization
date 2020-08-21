﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace MLM
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructor
		Organization Apple;

		public MainWindow()
		{
			InitializeComponent();
			Apple = new Organization("Apple Inc.");
			Apple.CreateRandomOrganization(5, 10, "", 100, null, Hierarchy.Top);
			Apple.CalculateTotalDeptSalary(PaymentType.Random);

		}

		#endregion

		#region On loaded
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var item = new TreeViewItem()
			{
				// Set the header
				Header = Apple.Name,

				// Add reference to the node content
				Tag = Apple
			};

			// Add a dummy item if the item being expanded has subitems
			// We do this in order to show "expand" arrow next to a tree node name
			// If a node cannot be expanded, arrow does not appear
			if ((item.Tag as Department).SubDepts.Count != 0)
				item.Items.Add(null);

			// Listen out for item being selected
			item.Selected += Department_Selected;
				
			// Listen out for item being expanded
			item.Expanded += Folder_Expanded;

			// Add it the main tree-view
			AppleTree.Items.Add(item);
		}
		#endregion

		#region Department selected
		private void Department_Selected(object sender, RoutedEventArgs e)
		{
			var es = e.OriginalSource as TreeViewItem;
			var dep = ((Department)es.Tag);
			// Binds employees of the current department to a DataGrid
			employeesView.ItemsSource = dep.Employees;
			
			// Shows total department salary at the bottom right corner of the window
			totalDeptSalary.Text = $"Total department salary: " +
				dep.TotalDepartmentSalary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
		}

		#endregion

		#region Department Expanded

		private void Folder_Expanded(object sender, RoutedEventArgs e)
		{
			#region Inittial Checks

			var item = (TreeViewItem)sender;

			// If the item only contains the dummy data
			if (item.Items.Count != 1 || item.Items[0] != null)
				return;

			// Clear dummy data
			item.Items.Clear();

			#endregion

			#region Get Departments

			// Create a blank list for directories
			var departments = ((Department)item.Tag).SubDepts;

			// For each dept ...
			departments.ForEach(dep =>
			{
				// Create department item
				var subItem = new TreeViewItem()
				{
					// Set the department name as a node texxt
					Header = dep.Name,

					// Set tag as a content of the current node
					Tag = dep
				};

				// Add a dummy item if the item being expanded has subitems
				// We do this in order to show "expand" arrow next to a tree node name
				// If a node cannot be expanded, arrow does not appear
				if ((subItem.Tag as Department).SubDepts.Count != 0)
					subItem.Items.Add(null);

				// Listen out for item being selected
				item.Selected += Department_Selected;

				// Handle expanding
				subItem.Expanded += this.Folder_Expanded;

				// Add this item to the parent
				item.Items.Add(subItem);
			});

			#endregion

		}


		#endregion

		#region Open & Save JSON files
		private void saveFileBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Apple == null) return;

			// Configure save file dialog box
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = "Apple"; // Default file name
			dlg.DefaultExt = ".json"; // Default file extension
			dlg.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

			// Show save file dialog box
			bool? result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result != true) return;

			// Save document
			JsonSerializer.Serialize(dlg.FileName, Apple);
		}
		private void openFileBtn_Click(object sender, RoutedEventArgs e)
		{
			// Configure open file dialog box
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.FileName = "CompanyName"; // Default file name
			dlg.DefaultExt = ".json"; // Default file extension
			dlg.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

			// Show open file dialog box
			bool? result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result != true) return;

			// Open document
			JsonSerializer.Deserialize(dlg.FileName);
		}

		#endregion

	}
}

#region Get Employees

//// Create a blank list for employees
//var employees = ((Department)item.Tag).Employees;


//// For each file ...
//employees.ForEach(w =>
//{
//	// Create file item
//	var subItem = new TreeViewItem()
//	{
//		DataContext = w,
//		// Set header as employee's name
//		Header = w,
//		//$"{w.FirstName} " + $"{w.LastName}" +
//		//		 $"{w.JobTitle} " + $"${w.Salary:### ###}",
//		// No more 
//		Tag = null
//	};

//	// Add this item to the parent
//	item.Items.Add(subItem);
//});

#endregion

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
//private int sal;
//public int Salary
//{
//	get { return sal;  }
//	set { }
//}

//public Test()
//{
//	this.sal = 400;
//}
//Test t = new Test();
//orgList = new ObservableCollection<string>()
//{
//	$"{t.Salary}"
//};
//t.Salary = 300;
//orgList.Add($"{t.Salary}");

//#region Helpers

///// <summary>
///// Find the file or folder name from a full path
///// </summary>
///// <param name="path">The full path</param>
///// <returns></returns>
//public static string GetFileFolderName(string path)
//{
//	// If we have no path, return empty
//	if (string.IsNullOrEmpty(path))
//		return string.Empty;

//	// Make all slashes back slashes
//	var normalizedPath = path.Replace('/', '\\');

//	// Find the last backslash in the path
//	var lastIndex = normalizedPath.LastIndexOf('\\');

//	// If we don't find a backslash, return the path itself
//	if (lastIndex <= 0)
//		return path;

//	// Return the name after the last back slash
//	return path.Substring(lastIndex + 1);
//}

//#endregion
#endregion
