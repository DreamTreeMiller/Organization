﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;


namespace MLM
{
	class Department
	{
		/// <summary>
		/// Current ID to assign to the next department
		/// </summary>
		private static uint staticID;

		/// <summary>
		/// Static constructor
		/// 
		/// </summary>
		static Department()
		{
			staticID = 0;
		}

		/// <summary>
		/// Generates next available ID
		/// </summary>
		/// <returns>New unique ID</returns>
		private static uint NextID()
		{
			staticID++;
			return staticID;
		}

		/// <summary>
		/// Unique department ID
		/// </summary>
		public uint DeptID { get; }

		/// <summary>
		/// Department name
		/// </summary>
		public string DeptName { get; set; }

		/// <summary>
		/// Department creation date
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// ID of parent department
		/// </summary>
		public uint ParentDept { get; set; }

		/// <summary>
		/// Collection of sub-departments' IDs
		/// </summary>
		public List<uint> SubDepts { get; set; }

		/// <summary>
		/// Number of department employees
		/// I'm not sure if we need this field. It will require addition logic to update it
		/// But may be good to keep for consistency purposes
		/// </summary>
		public int NumberOfEmployees { get; set; }

		/// <summary>
		/// Sum of salaries received by
		/// - all dept workers - employees & interns
		/// - all sub-departments
		/// - director. 
		/// Director's salary is 15% of salaries of all dept workers and all sub-departments
		/// /// </summary>
		public uint TotalDepartmentSalary { get; set; }

		/// <summary>
		/// Constructor to create dummy worker with particular ID in order to check 
		/// if a worker with such ID already exists
		/// </summary>
		/// <param name="workerID"></param>
		public Department(uint deptID)
		{
			this.DeptID = deptID;
		}

		/// <summary>
		/// Constructor create an instance of the Department class
		/// </summary>
		/// <param name="deptName"></param>
		/// <param name="parentDept"></param>
		public Department(string deptName, uint parentDept)
		{
			DeptID					= NextID();
			DeptName				= deptName;
			CreatedOn				= DateTime.Now;
			ParentDept				= parentDept;
			SubDepts				= new List<uint>();
			NumberOfEmployees		= 0;
			TotalDepartmentSalary	= 0;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// This line is necessary because DisconnectedItem exception is thrown
			// when we change department in TreeView
			// But I don't know why DataGrid calls Equals method ...
			if (obj == BindingOperations.DisconnectedSource) return false;
			Department d = obj as Department;
			return this.DeptID.Equals(d.DeptID);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return $"ID: {DeptID,16} " +
					$"{DeptName,20} " +
					$"Created on: {CreatedOn:dd.MM.yyyy} " +
					$"Employees: {NumberOfEmployees:7} " +
					$"Total Salary: ${TotalDepartmentSalary:### ### ###}";
		}
	}
}

#region Not clear if I need it
//public ObservableCollection<string> MakeObsCollection(string pad)
//{
//	ObservableCollection<string> dptList = new ObservableCollection<string>
//	{
//		pad + this
//	};
//	foreach (Worker w in Employees)
//		dptList.Add(pad + "  " + w);
//	foreach (Department d in SubDepts)
//		dptList = ConcatCollections(dptList,(d.MakeObsCollection(pad + "    ")));
//	return dptList;
//}

//ObservableCollection<string> ConcatCollections(ObservableCollection<string> body,
//											ObservableCollection<string> tail)
//{
//	foreach (var s in tail)
//		body.Add(s);
//	return body;
//}

#endregion
