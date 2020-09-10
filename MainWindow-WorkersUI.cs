using MLM.Interfaces;
using MLM.InterfacesActions;
using MLM.ManipulationsOnWorkers;
using MLM.ActionsBackEnd;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MLM.Organizaton;

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
		private void AddWorker_Click(object sender, RoutedEventArgs e)
		{
			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;

			// For each hierarchy level provide proper list of available positions
			// i.e. you can't add Head of the Division at the Department level
			var availablePositionsList = Apple.Available(d, false);

			// Invoke Add Worker Menu window
			AddWorkerMenu addWorkerWin = new AddWorkerMenu(d.DeptName, availablePositionsList);

			bool? result = addWorkerWin.ShowDialog();
			if (result != true) return;

			// Pass collected worker's information to the back-end worker adding method
			// Cast interface values to back-end types
			int addResult = UI.Workers.AddWorker(
				addWorkerWin.FirstNameEntryBox.Text,                                    // First Name
				addWorkerWin.LastNameEntryBox.Text,                                     // Last Name
				(DateTime)addWorkerWin.DateOfBirthPicker.SelectedDate,                  // Date of birth
				d,																		// Department
				(addWorkerWin.PositionEntryBox.SelectedItem as IPositionTuple).Pos      // Position
				);

			// Check if adding was successful if not display a warning message
			if (addResult != 0)
			{
				addWorkerWin.AddingMistakeWarning(addResult);
				return;
			}

			UpdateMainWindow();
		}

		private void EditWorker_Click(object sender, RoutedEventArgs e)
		{
			// Worker to be edited
			var w = WorkersView.SelectedItem as IWorkerDTO;
			if (w == null) return;

			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var d = tvis.Tag as IDepartmentDTO;

			// Check if deparment has director. 
			// This is necessary to calculate salaries in EditWorker Window
			bool hasDirector = true;
			if ((Apple as IRetrieve).Director(d) == null) hasDirector = false;

			// For each hierarchy level provide proper list of available positions
			// i.e. you can't add Head of the Division at the Department level
			bool keepDirector = false;
			if (w is IDirector) keepDirector = true;
			var availablePositionsList = Apple.Available(d, keepDirector);

			// Open Edit Worker dialog window
			EditWorkerMenu editWorkerWin = 
				new EditWorkerMenu(w, d, hasDirector, 
				availablePositionsList,
				(Apple as IRetrieve).RootDepartment().CreatedOn);
			bool? result = editWorkerWin.ShowDialog();

			if (result != true) return;

			UI.Workers.EditWorker(editWorkerWin.wCopy);

			UpdateMainWindow();
		}

		/// <summary>
		/// Moves worker from one dept to another
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MoveWorker_Click(object sender, RoutedEventArgs e)
		{
			//Worker to be moved
			var ws = WorkersView.SelectedItem as IWorkerDTO;
			if (ws == null) return;

			// Get currently selected department from the Tree View
			var tvis = AppleTree.SelectedItem as TreeViewItem;
			if (tvis == null) return;
			var currDept = tvis.Tag as IDepartmentDTO;

			var deptTable = UI.Get.DepartmentsList();

			// Take out current department from the list of destination departments
			deptTable.Remove(currDept);

			// Open Move Worker dialog window
			MoveWorker moveWorkerWin = new MoveWorker(ws, currDept.DeptName, deptTable);

			bool? result = moveWorkerWin.ShowDialog();
			if (result != true) return;

			// Get destination department of the being moved worker from ComboBox selection
			var newDept = moveWorkerWin.DeparmmentEntryBox.SelectedItem as BaseDepartment;

			// Move worker to another department. 
			// Salaries of current and destination departments, and departments above, will be updated
			UI.Workers.MoveWorker(ws, newDept);

			UpdateMainWindow();
		}

		private void DeleteWorker_Click(object sender, RoutedEventArgs e)
		{
			IWorkerDTO ws = WorkersView.SelectedItem as IWorkerDTO;
			if (ws == null) return;
			DeleteItemConfirmationDialog delCon =
				new DeleteItemConfirmationDialog(
					$"Are you sure you want to delete worker:\n\n" +
					$"ID: {ws.ID}, {ws.FirstName} {ws.LastName} ?\n\n" +
					"Worker will be deleted permnanently!");
			bool? result = delCon.ShowDialog();

			if (result != true) return;
			UI.Workers.RemoveWorker(ws);

			UpdateMainWindow();
		}

	}
}
