using MLM.Interfaces;
using MLM.InterfacesActions;
using MLM.UI_DepartmentsActions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MLM
{
	/// <summary>
	/// Manipulations on workers user interface
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Creates new worker
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CreateDepartment_Click(object sender, RoutedEventArgs e)
		{
			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;

			string newDeptName = d.DeptName + $"_{d.NumberOfSubDepts + 1}";
			if (d is IHQ)
				newDeptName = "Division" + $"_{d.NumberOfSubDepts + 1}";

			if (d is IDivision)
				newDeptName = "Department" + $"_{d.NumberOfSubDepts + 1}";

			// Invoke Create Department Menu window
			CreateDepartmentMenu createDeptWin = 
				new CreateDepartmentMenu(newDeptName, Apple.EstablishedOn, (Apple as IRetrieve).SubDepartments(d));

			bool? result = createDeptWin.ShowDialog();
			if (result != true) return;

			var newDept = UI.Departments.CreateDepartment(
				d,
				createDeptWin.DeptName,
				createDeptWin.DeptCreationDate);

			var subItem = CreateTVINode(newDept);

			// Add this item to the parent
			tvis.Items.Add(subItem);
			tvis.Items.Refresh();
		}

		/// <summary>
		/// Invokes edit department window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditDepartment_Click(object sender, RoutedEventArgs e)
		{
			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;

			// Invoke Edit Department Menu window
			EditDepartmentMenu editDeptWin =
				new EditDepartmentMenu(
					d,
					Apple.EstablishedOn,
					(Apple as IRetrieve).SubDepartments(d));

			bool? result = editDeptWin.ShowDialog();
			if (result != true) return;

			// To keep consistency of separating UI and back-end
			// Let's call EditDepartment method
			// Right now this method is redundant
			UI.Departments.EditDepartment(d, editDeptWin.DeptName, editDeptWin.DeptCreationDate);
			
			// Actually, we don't even need to do like this
			//d.DeptName = editDeptWin.DeptName;
			//d.CreatedOn = editDeptWin.DeptCreationDate;
			// because a reference to the source data was passed to the 
			// Edit Department Window
			// Probably this is not good practice

			// Updating department name in the UI TreeView
			tvis.Header = editDeptWin.DeptName;
			tvis.Items.Refresh();
		}

		private void MoveDepartment_Click(object sender, RoutedEventArgs e)
		{
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;		// No department is selected

			// Get currently selected department and its parent from the Tree View
			var d = tvis.Tag as IDepartmentDTO;
			var parent = tvis.Parent as TreeViewItem;

			var availableDestinations = 
				UI.Departments.AvailableDestinationDepartmentsList(d);
			var moveDeptWin = new MoveDepartmentMenu(d, availableDestinations);
			var result = moveDeptWin.ShowDialog();

			if (result != true) return;

			var destDept = moveDeptWin.destDept;

			var moved_Dept = UI.Departments.MoveDepartment(d, destDept);
			parent.Items.Remove(tvis);
			parent.Items.Refresh();

			var newParent = FindNode(AppleTreeRoot, destDept);

			// If item was never shown in TreeView then newParent TVI will be null
			if (newParent == null) return;

			// Update tree view to show moved dept
			var subItem = CreateTVINode(moved_Dept);

			// Add this item to the parent
			newParent.Items.Add(subItem);
			newParent.Items.Refresh();
		}

		/// <summary>
		/// Searches through UI Tree for specific node
		/// </summary>
		/// <param name="entryNode"></param>
		/// <param name="d"></param>
		/// <returns>UI TreeViewItem corresponding to department</returns>
		private TreeViewItem FindNode(TreeViewItem entryNode, IDepartmentDTO d)
		{
			var currDept = entryNode.Tag as IDepartmentDTO;
			if (currDept.DeptID == d.DeptID)
				return entryNode;
			TreeViewItem foundNode = null;
			foreach (var subNode in entryNode.Items)
			{
				// We need this check because our TreeViewItem.Items has null item at Items[0] 
				// in shown but not expanded yet items.
				if (subNode == null) continue;
				foundNode = FindNode(subNode as TreeViewItem, d);
				if (foundNode != null) break;
			}
			return foundNode;
		}

		/// <summary>
		/// Performs all necessary checking to delete department,
		/// then calls DeleteDepartment method of back-end
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteDepartment_Click(object sender, RoutedEventArgs e)
		{
			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;
			var parent = tvis.Parent as TreeViewItem;

			// Check if we are not deleting root department
			if (d.ParentDeptID == 0)
			{
				MessageBox.Show("You cannot delete HQ department!");
				return;
			}

			// Are we trying to delete empty department
			bool? result;
			if (d.NumberOfEmployees == 0 &&
				d.NumberOfEmployees == 0)
			{
				DeleteItemConfirmationDialog delWin = new
					DeleteItemConfirmationDialog(
						"Are you sure to delete department\n" +
						$"{d.DeptName} ?\n" +
						"This action cannot be undone.");
				result = delWin.ShowDialog();
				if (result != true) return;
				UI.Departments.DeleteEmptyDepartment(d);

				// Remove department from UI TreeView
				parent.Items.Remove(tvis);
			}
			else	// Department has either employees or subdepartments or both
			{
				DeleteDepartmentMenu delDeptWin = new DeleteDepartmentMenu(d);
				result = delDeptWin.ShowDialog();
				if (result != true) return;

				if (delDeptWin.selfExcludeOrCompletely) // Self Exclude way
				{
					DeleteItemConfirmationDialog delWin = new
						DeleteItemConfirmationDialog(
							"Are you sure to delete department\n" +
							$"{d.DeptName} ?\n" +
							"This action cannot be undone.");
					result = delWin.ShowDialog();
					if (result != true) return;
					UI.Departments.SelfExcludeOfDepartment(d);
					
					// Update UI TreeView
					parent.Items.Clear();

					// Refresh list of subdepartments
					// Because some may were added manually
					var subDepartments = UI.Get.SubDepartments((IDepartmentDTO)parent.Tag);

					// For each dept ...
					subDepartments.ForEach(dept =>
					{
						// Create department item
						var subItem = CreateTVINode(dept);

						// Add this item to the parent
						parent.Items.Add(subItem);
					});
				}
				else  // Delete completely with all employees and subdepartments
				{
					DeleteItemConfirmationDialog delWin = new
						DeleteItemConfirmationDialog(
							$"Are you sure to completely delete department {d.DeptName}\n" +
							"and all its sub departments with all their employees?\n" +
							"This action cannot be undone.");
					result = delWin.ShowDialog();
					if (result != true) return;

					MessageBox.Show("Remove completely!!!");
					UI.Departments.DeleteCompletely(d);

					// Remove department from UI TreeView
					parent.Items.Remove(tvis);
				}
			}

			parent.Items.Refresh();
		}
	}
}
