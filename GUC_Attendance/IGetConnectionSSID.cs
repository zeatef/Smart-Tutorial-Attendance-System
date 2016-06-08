// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;

namespace GUC_Attendance
{
	public interface IGetConnectionSSID
	{
		string getSSID ();

		int getIP ();

		bool IsConnectedToInternet ();
	}
}

