using System;
using Xamarin.Forms;
using System.Net.Sockets;
using GUC_Attendance.iOS;
using System.Net;
using System.Diagnostics;
using GUC_Attendance.Models;
using System.Collections.Generic;

[assembly: Dependency (typeof(SocketProgramming_iOS))]


namespace GUC_Attendance.iOS
{
	public class SocketProgramming_iOS : ISocketProgramming
	{

		Server_iOS serverSocket;
		Socket clientSocket;

		public void SetServerSocket (SQLDatabase db, enroll_view e, int w_no, int ipaddress, IEnumerable<WeeklyAttendance> itemssource)
		{
			Debug.WriteLine (ipaddress);
			IPAddress ip = new IPAddress (ipaddress);
			serverSocket = new Server_iOS (ip);
		}

		public void SetClientSocket (string id)
		{
			serverSocket = new Server_iOS (new IPAddress (12345));
		}

		public void CloseServerSocket ()
		{
			
		}
	}
}

