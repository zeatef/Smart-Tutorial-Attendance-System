using System;
using Xamarin.Forms;
using System.Net.Sockets;
using GUC_Attendance.Droid;
using System.Net;
using System.Diagnostics;
using GUC_Attendance.Models;
using System.Collections.Generic;

[assembly: Dependency (typeof(SocketProgramming_Android))]

namespace GUC_Attendance.Droid
{
	public class SocketProgramming_Android : ISocketProgramming
	{

		Server_Android serverSocket;
		Client_Android clientSocket;

		public void SetServerSocket (SQLDatabase db, enroll_view e, int w_no, int ipaddress)
		{
			Debug.WriteLine (ipaddress);
			IPAddress ip = new IPAddress (ipaddress);
			serverSocket = new Server_Android (db, e, w_no, ip);
			serverSocket.StartServer ();
		}

		public void CloseServerSocket ()
		{
			serverSocket.StopServer ();
		}

		public void SetClientSocket (string id, int ipaddress)
		{
			clientSocket = new Client_Android (id, ipaddress);
		}
	}
}

