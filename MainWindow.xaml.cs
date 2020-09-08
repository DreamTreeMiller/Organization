using MLM.Interfaces;
using MLM.ActionsUserInterface;
using MLM.InterfacesActions;

using MLM.ManipulationsOnWorkers;
using MLM.ActionsBackEnd;
using System;
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
		private ActionsUI UI;			// Actions on organization user interface.
										// Does not depend on how organization is implemented

		public MainWindow()
		{
			InitializeComponent();
			ICreateOrganization Create = new CreateOrganization();
			Apple = Create.Organization(5, 5, "Apple Inc.", 30);
			UI = new ActionsUI(Apple);
		}

		#endregion

		#region On loaded
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Get root department
			IDepartmentDTO d = UI.Get.RootDepartment();

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

			// Add it the main tree-view
			AppleTree.Items.Add(item);
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
			#region Inittial Checks

			var item = (TreeViewItem)sender;

			// If the item only contains the dummy data
			if (item.Items.Count != 1 || item.Items[0] != null)
				return;

			// Clear dummy data
			item.Items.Clear();

			#endregion

			#region Get Sub Departments

			// Create a blank list for directories
			var subDepartments = UI.Get.SubDepartments((IDepartmentDTO)item.Tag);

			// For each dept ...
			subDepartments.ForEach(d =>
			{
				// Create department item
				var subItem = new TreeViewItem()
				{
					// Set the department name as a node texxt
					Header = d.DeptName,

					// Set tag as a content of the current node
					Tag = d
				};

				// Add a dummy item if the item being expanded has subitems
				// We do this in order to show "expand" arrow next to a tree node name
				// If a node cannot be expanded, arrow does not appear
				if ((subItem.Tag as IDepartmentDTO).NumberOfSubDepts != 0)
					subItem.Items.Add(null);

				// Listen out for item being selected
				item.Selected += Department_Selected;

				// Handle expanding
				subItem.Expanded += this.Department_Expanded;

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
			if (dir != null)
				DirectorSalary.Text =
					dir.Salary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
			else
				DirectorSalary.Text = "$0";

			StaffSalary.Text =
				d.TotalDeptStaff_withoutBoss_Salary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US")) +
				$" ({d.NumberOfEmployees - 1} ppl)";

			SubDepartmentsSalary.Text =
				d.TotalSubDepartmentsSalary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));

			TotalDepartmentSalary.Text =
				d.TotalDepartmentSalary.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
		}

		#endregion

		#region Manipulations with department

		private void CreateDepartment_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Create department");
		}

		private void EditDepartment_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Edit department");
		}

		private void MoveDepartment_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Move department");
		}

		private void DeleteDepartment_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Delete department");
		}

		#endregion
	}
}

		/*
			MessageBox.Show(
				$"TreeView Selected Item: {AppleTree.SelectedItem}\n" +
				$"DataGrid Selected Item: {employeesView.SelectedItem}" 
				);
		 */

