using System;
using System.Net.Sockets;
using Android.OS;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Text;
using Java.Security;
using Acr.UserDialogs;



namespace GUC_Attendance.Droid
{
	public class Client_Android
	{
		TcpClient client;
		string id;
		int intip;

		public Client_Android (string id, int intip)
		{
			try {
				this.id = id;
				this.intip = intip;
				client = new TcpClient ();
				this.Connect ();
			} catch (Exception e) {
				UserDialogs.Instance.Alert (e.Message);
			}
		}

		public void Connect ()
		{
			
			IPAddress ipaddress = new IPAddress (intip);
			client = new TcpClient ();
			client.Connect (ipaddress, 1994);
			var thread = new Thread (
				             () => {
					NetworkStream networkstream = client.GetStream ();
					Byte[] buffer = Encoding.ASCII.GetBytes (id);
					networkstream.Write (buffer, 0, buffer.Length);
					Thread.Sleep (500);
					try {
						while (!networkstream.DataAvailable) {
							networkstream.Write (buffer, 0, buffer.Length);
							System.Diagnostics.Debug.WriteLine ("PING!!!");
							Thread.Sleep (10000);
						}
					} catch (Exception ee) {
						UserDialogs.Instance.Alert (ee.Message);
					} finally {
						networkstream.Close ();
						client.Close ();
					}

				});
			thread.Start ();
		}
	}

}

