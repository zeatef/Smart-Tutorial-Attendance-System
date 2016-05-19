using System;
using GUC_Attendance.Models;

namespace GUC_Attendance
{
	public interface ISocketProgramming
	{
		void SetServerSocket (SQLDatabase db, enroll_view e, int w_no, int ipaddress);

		void CloseServerSocket ();

		void SetClientSocket (string id);

	}
}

