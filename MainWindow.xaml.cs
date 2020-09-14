﻿using MLM.Interfaces;
using MLM.ActionsUserInterface;
using MLM.InterfacesActions;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace MLM
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructor

		private IOrganization Apple;
		private Organizaton.Organization org;
		private ActionsUI UI;           // Actions on organization user interface.
										// Does not depend on how organization is implemented

		public MainWindow()
		{
			InitializeComponent();
			ICreateOrganization Create = new CreateOrganization();
			Apple = Create.Organization(5, 5, "Apple Inc.", 30);
			org = Apple as Organizaton.Organization;
			this.Title = "Apple Inc.";
			this.OrgEstablishedOn.Text = 
				$"Established on " +
				Apple.EstablishedOn.ToString("MMMM dd, yyyy",
				  CultureInfo.CreateSpecificCulture("en-US"));
			UI = new ActionsUI(Apple);

			AllDepartments.ItemsSource = (Apple as Organizaton.Organization).Departments;
			AllWorkers.ItemsSource = (Apple as Organizaton.Organization).Workers;
		}

		#endregion

		TreeViewItem AppleTreeRoot;

		/// <summary>
		/// Creates TreeViewItem node for the department
		/// </summary>
		/// <param name="d">Department</param>
		/// <returns></returns>
		private TreeViewItem CreateTVINode(IDepartmentDTO d)
		{
			var item = new TreeViewItem()
			{
				// Set the header
				Header = d.DeptName,

				// Add reference to the node content
				Tag = d
			};

			// Add a dummy item if the item being expanded has subitems
			// We do this in order to show "expand" arrow next to a tree node name
			// If a node cannot be expanded, arrow does not appear
			if ((item.Tag as IDepartmentDTO).NumberOfSubDepts != 0)
				item.Items.Add(null);

			// Listen out for item being selected
			item.Selected += Department_Selected;

			// Listen out for item being expanded
			item.Expanded += Department_Expanded;

			return item;
		}

		#region On loaded
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Get root department
			IDepartmentDTO d = UI.Get.RootDepartment();

			var item = CreateTVINode(d);

			// Add it the main tree-view
			AppleTree.Items.Add(item);
			AppleTreeRoot = item;
		}
		#endregion

		#region Department selected
		private void Department_Selected(object sender, RoutedEventArgs e)
		{
			var es = e.OriginalSource as TreeViewItem;
			var d = (IDepartmentDTO)es.Tag;

			// Binds employees of the current department to a DataGrid
			WorkersView.ItemsSource = UI.Get.OneDepartmentWorkersList(d);

			UpdateMainWindow();
		}

		#endregion

		#region Department Expanded

		private void Department_Expanded(object sender, RoutedEventArgs e)
		{
			var item = (TreeViewItem)sender;

			// If the first item is dummy data (item.Items[0] == null)
			// It means there is/are subdepartment(s)
			// We need to add them to TreeViewItem
			if (item.Items[0] != null)
				return;

			// Clear all data
			item.Items.Clear();

			// Refresh list of subdepartments
			// Because some may were added manually
			var subDepartments = UI.Get.SubDepartments((IDepartmentDTO)item.Tag);

			// For each dept ...
			subDepartments.ForEach(d =>
			{
				// Create department item
				var subItem = CreateTVINode(d);

				// Add this item to the parent
				item.Items.Add(subItem);
			});
		}


		#endregion

		#region Open & Save JSON files
		private void saveFileBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Apple == null) return;
			UI.Json.Serialize(Apple);
		}
		private void openFileBtn_Click(object sender, RoutedEventArgs e)
		{
			Apple = UI.Json.Deserialize();
		}

		#endregion

		#region Update workers list and bottom dashboard

		/// <summary>
		/// Shows salaries of director, staff, sub departments, department
		/// </summary>
		/// <param name="d"></param>
		private void UpdateMainWindow()
		{
			// Get current department from the treeview
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;

			// Binds employees of the current department to a DataGrid
			WorkersView.ItemsSource = UI.Get.OneDepartmentWorkersList(d);

			// Shows salaries at the bottom of the window
			var dir = UI.Get.Director(d);

			// Put proper position title for the department boss
			BossTitle.Text = Apple.PositionsData[Positions.DeptDirector] + ": ";
			if (d is IHQ)		BossTitle.Text = Apple.PositionsData[Positions.President] + ": ";
			if (d is IDivision)	BossTitle.Text = Apple.PositionsData[Positions.DivisionHead] + ": ";

			// Check if a department has director
			int dirPresent = 1;
			if (dir != null)
				DirectorSalary.Text =
					dir.Salary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
			else
			{
				DirectorSalary.Text = "$0";
				dirPresent--;
			}

			StaffSalary.Text =
				d.TotalDeptStaff_withoutBoss_Salary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US")) +
				$" ({d.NumberOfEmployees - dirPresent} ppl)";

			SubDepartmentsSalary.Text =
				d.TotalSubDepartmentsSalary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));

			TotalDepartmentSalary.Text =
				d.TotalDepartmentSalary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
		}

		#endregion
	}
}
