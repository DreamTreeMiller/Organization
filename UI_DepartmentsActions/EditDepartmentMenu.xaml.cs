using MLM.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MLM.UI_DepartmentsActions
{
	/// <summary>
	/// Interaction logic for EditDepartmentMenu.xaml
	/// </summary>
	public partial class EditDepartmentMenu : Window
	{
		List<IDepartmentDTO> _subDepts;
		string _deptname;
		public string DeptName
		{
			get => _deptname;
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					MessageBox.Show("Please enter name!");
					return;
				}

				var deptWithNewName = _subDepts.Find(d => d.DeptName == value);
				if (deptWithNewName != null)
				{
					MessageBox.Show("Sub department with such name already exists!\n" +
									"Please enter another name.");
					return;
				}
				_deptname = value;
			}
		}
		DateTime orgcreationdate;
		DateTime _deptcreationdate = DateTime.Now;
		public DateTime DeptCreationDate
		{
			get => _deptcreationdate;
			set
			{
				if (value < orgcreationdate)
				{
					MessageBox.Show("Date should be equal or later\n" +
									"than organiztion creation date\n" +
									$"{orgcreationdate:D}");
					return;
				}
				if (value > DateTime.Now)
				{
					MessageBox.Show("Date should be equal or earlier\n" +
									"than today.");
					return;
				}
				_deptcreationdate = value;
				DeptNameEntryBox.SelectionStart = DeptNameEntryBox.Text.Length;
			}
		}

		public EditDepartmentMenu(IDepartmentDTO d,
								  DateTime orgCreationDate,
								  List<IDepartmentDTO> subDepts)
		{
			InitializeComponent();
			_subDepts = subDepts;
			DeptName = d.DeptName;
			orgcreationdate = orgCreationDate;
			DeptCreationDate = d.CreatedOn;
			EditDeptGrid.DataContext = this;
		}

		private void btnOk_EditDepartment_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
