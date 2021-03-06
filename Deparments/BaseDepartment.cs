﻿using MLM.Interfaces;
using System;
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
	 public abstract class BaseDepartment : IDepartmentDTO
	{
		/// <summary>
		/// Current ID to assign to the next department
		/// </summary>
		private static uint staticID;

		/// <summary>
		/// Static constructor
		/// 
		/// </summary>
		static BaseDepartment()
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
		public uint ParentDeptID { get; set; }

		public int NumberOfSubDepts { get => SubDepts.Count; }

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
		public int TotalDepartmentSalary { get; set; }

		public int TotalDeptStaff_withoutBoss_Salary { get; set; }
		public int TotalSubDepartmentsSalary { get; set; }

		/// <summary>
		/// Constructor to create dummy worker with particular ID in order to check 
		/// if a worker with such ID already exists
		/// </summary>
		/// <param name="deptID">ID of department</param>
		public BaseDepartment(uint deptID)
		{
			this.DeptID = deptID;
		}

		/// <summary>
		/// Constructor create an instance of the Department class
		/// </summary>
		/// <param name="deptName"></param>
		/// <param name="parentDept"></param>
		public BaseDepartment(string deptName, uint parentDeptID)
		{
			DeptID					= NextID();
			DeptName				= deptName;
			CreatedOn				= DateTime.Now;
			ParentDeptID			= parentDeptID;
			SubDepts				= new List<uint>();
			NumberOfEmployees		= 0;
			// Total Department salary is sum of:
			// - Director's salary
			// - Sum of salaries of all department staff except director
			// - Sum of Total Department salaries of all sub departments
			TotalDepartmentSalary	= 0;

			// Sum of salaries of all department staff except director
			TotalDeptStaff_withoutBoss_Salary = 0;

			// Sum of Total Department salaries of all sub departments
			TotalSubDepartmentsSalary = 0;
		}

		/// <summary>
		/// Creates copy of the department. Need this constructor to change type
		/// </summary>
		/// <param name="d">Department to be copied</param>
		public BaseDepartment(BaseDepartment d)
		{
			DeptID								= d.DeptID;
			DeptName							= d.DeptName;
			CreatedOn							= d.CreatedOn;
			ParentDeptID						= d.ParentDeptID;
			NumberOfEmployees					= d.NumberOfEmployees;
			TotalDepartmentSalary				= d.TotalDepartmentSalary;
			TotalDeptStaff_withoutBoss_Salary	= d.TotalDeptStaff_withoutBoss_Salary;
			TotalSubDepartmentsSalary			= d.TotalSubDepartmentsSalary;

			SubDepts = new List<uint>();
			foreach (uint sd in d.SubDepts) SubDepts.Add(sd);
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
