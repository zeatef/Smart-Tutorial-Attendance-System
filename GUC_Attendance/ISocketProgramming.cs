using System;
using GUC_Attendance.Models;
using System.Collections.Generic;

namespace GUC_Attendance
{
	public interface ISocketProgramming
	{
		void SetServerSocket (SQLDatabase db, enroll_view e, int w_no, int ipaddress, IEnumerable<WeeklyAttendance> itemssource);

		void CloseServerSocket ();

		void SetClientSocket (string id);

	}
}

