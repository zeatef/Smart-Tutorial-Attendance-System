using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

namespace GUC_Attendance.iOS
{
	public class Server_iOS
	{

		TcpListener server;
		TcpClient client;

		public Server_iOS (IPAddress ipaddress)
		{
			try {
				server = new TcpListener (ipaddress, 6000);
				Debug.WriteLine ("Server Welcomes you, Listening...");
				while (true) {
					client = server.AcceptTcpClient ();
				}
			} catch (Exception e) {
				e.Message.ToString ();
			}
		}
	}
}

