// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using GUC_Attendance.Models;
using System.Collections.Generic;

namespace GUC_Attendance
{
	public interface ISocketProgramming
	{
		void SetServerSocket (SQLDatabase db, enroll_view e, int w_no, int ipaddress);

		void CloseServerSocket ();

		void SetClientSocket (string id);

	}
}

