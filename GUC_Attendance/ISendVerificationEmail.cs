// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;

namespace GUC_Attendance
{
	public interface ISendVerificationEmail
	{
		void sendEmail(string fname, string lname, string recipient, int verificatoncode);
	}
}

