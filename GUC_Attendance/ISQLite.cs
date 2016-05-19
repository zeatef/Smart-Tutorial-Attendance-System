// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using SQLite.Net;

namespace GUC_Attendance
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}