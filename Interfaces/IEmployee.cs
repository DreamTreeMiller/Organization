﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	/// Interface alias for Employee worker type
	public interface IEmployee : IWorkerDTO
	{
		int HourlyRate { get; set; }
		int HoursWorked { get; set; }
		//object Clone();
	}
}
