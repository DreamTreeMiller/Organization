using System;
using MLM.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MLM
{
	/// <summary>
	/// Data Transfer Object Class to recieve amended eployee's data in UI 
	/// and pass to back-end
	/// </summary>
	public class EditWorkerValidation : INotifyPropertyChanged, IWorkerDTO
	{
		WorkerHierarchy IWorkerDTO.OriginalWorkerType { get; set; }
		WorkerHierarchy IWorkerDTO.SelectedWorkerType { get; set; }

		public uint ID { get; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		private DateTime _dateofbirth;
		public DateTime DateOfBirth
		{
			get => _dateofbirth;
			set
			{
				if (DateTime.Now.Subtract(value).Days / 365.25 < 18)
				{
					MessageBox.Show("Person should have been born at least 18 years ago!");
					return;
				}

				if (DateTime.Now.Subtract(value).Days / 365.25 > 70)
				{
					MessageBox.Show("Person elder than 70 cannot work in the company!");
					return;
				}
				_dateofbirth = value;
			}
		}

		private readonly DateTime orgCreationDate;
		private DateTime _employedon;
		public DateTime EmployedOn
		{
			get => _employedon;
			set
			{
				if (value > DateTime.Now)
				{
					MessageBox.Show("Employment date should be today or earlier.");
					return;
				}

				if (value < orgCreationDate)
				{
					MessageBox.Show("Person cannot be hired before organiation was created.");
					return;
				}
				_employedon = value;
			}
		}
		/// <summary>
		/// ID of a department worker works in
		/// </summary>
		public uint DeptID { get; set; }

		/// <summary>
		/// Position - enum type of several possible posisitons
		/// withing organization President, VP, Head of Division, Vice Head, Dept Director, Vice Dir, Employee, Intern
		/// </summary>
		public Positions Position { get; set; }       // 

		/// <summary>
		/// Current text of position.
		/// Amended by EditWorker method based on Position property value and 
		/// department of worker
		/// </summary>
		public string PositionTitle { get; set; }

		/// <summary>
		/// To keep compatability with IWorker interface
		/// Worker's salary calculated based on salaryBase or other parameters
		/// </summary>
		public int Salary { get; set; }

		private int _dirsalary = 1300;
		public int DirSalary
		{
			get => _dirsalary;
			set
			{
				if (value < 1300) value = 1300;
				_dirsalary = value;
			}
		}
		public int EmpSalary
		{
			get { return HourlyRate * HoursWorked; }
			set { }
		}

		private int _hourlyrate = 12;
		public int HourlyRate
		{
			get => _hourlyrate;
			set
			{
				if (value < 1)
					_hourlyrate = 1;
				else
					_hourlyrate = value;
				NotifyPropertyChanged("EmpSalary");
			}
		}

		private int _hoursworked = 22 * 8;
		public int HoursWorked
		{
			get => _hoursworked;
			set
			{
				if (value < 0)
					_hoursworked = 0;
				else if (value > 744)
					_hoursworked = 744;
				else
					_hoursworked = value;
				NotifyPropertyChanged("EmpSalary");
			}
		}

		private int _intsalary = 500;
		public int IntSalary
		{
			get => _intsalary;
			set
			{
				if (value < 0)
					_intsalary = 0;
				else if (value > 1000)
					_intsalary = 1000;
				else
					_intsalary = value;
			}
		}

		public EditWorkerValidation(IWorker w, IDepartmentDTO d, bool hasDirector, DateTime orgCreationDate)
		{
			this.ID = w.ID;
			this.FirstName = w.FirstName;
			this.LastName = w.LastName;
			this.DateOfBirth = w.DateOfBirth;
			this.orgCreationDate = orgCreationDate;
			this.EmployedOn = w.EmployedOn;
			this.DeptID = w.DeptID;
			this.Position = w.Position;
			this.PositionTitle = w.PositionTitle;

			if (w is Director)
			{
				(this as IWorkerDTO).OriginalWorkerType = WorkerHierarchy.Director;
				DirSalary = w.Salary;
			}

			// Calculating director's salary to show in the form
			// This is necessary if department currently does not have director
			// and we need to show salary of a worker if he will become a director
			if (hasDirector == false)
			{
				DirSalary =
					(d.TotalDeptStaff_withoutBoss_Salary +
					 d.TotalSubDepartmentsSalary -
					 w.Salary) / 100 * 15;
			}

			if (w is IEmployee)
			{
				(this as IWorkerDTO).OriginalWorkerType = WorkerHierarchy.Employee;
				HourlyRate  = (w as IEmployee).HourlyRate;
				HoursWorked = (w as IEmployee).HoursWorked;
			}

			if (w is IIntern)
			{
				(this as IWorkerDTO).OriginalWorkerType = WorkerHierarchy.Intern;
				IntSalary = w.Salary;
			}

			(this as IWorkerDTO).SelectedWorkerType = (this as IWorkerDTO).OriginalWorkerType;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.  
		// The CallerMemberName attribute that is applied to the optional propertyName  
		// parameter causes the property name of the caller to be substituted as an argument.  
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		//public object Clone()
		//{
		//	return (IWorker) this.MemberwiseClone();
		//}
	}

}
