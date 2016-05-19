using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Android.OS;

namespace GUC_Attendance.Droid
{
	public class ClientAsync_Android
	{
		Socket clientSocket;

		class StateObject
		{
			internal byte[] sBuffer;
			internal Socket sSocket;

			internal StateObject (int size, Socket sock)
			{
				sBuffer = new byte[size];
				sSocket = sock;
			}
		}

		public ClientAsync_Android ()
		{
			IPAddress ipaddress = new IPAddress (101323948);
			IPEndPoint endpoint = new IPEndPoint (ipaddress, 6000);
			clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			IAsyncResult asyncConnect = clientSocket.BeginConnect (endpoint, new AsyncCallback (connectCallback), clientSocket);

		}

		public void connectCallback (IAsyncResult asyncConnect)
		{
			Socket clientSocket = (Socket)asyncConnect.AsyncState;
			clientSocket.EndConnect (asyncConnect);

			// arriving here means the operation completed
			// (asyncConnect.IsCompleted = true) but not
			// necessarily successfully
			if (clientSocket.Connected == false) {
				System.Diagnostics.Debug.WriteLine ("Client is not connected.");
				return;
			} else
				System.Diagnostics.Debug.WriteLine ("Client is connected.");

			byte[] sendBuffer = Encoding.ASCII.GetBytes ("PING!!!");
			while (true) {
				IAsyncResult asyncSend = clientSocket.BeginSend (
					                         sendBuffer,
					                         0,
					                         sendBuffer.Length,
					                         SocketFlags.None,
					                         new AsyncCallback (sendCallback),
					                         clientSocket);

				System.Diagnostics.Debug.WriteLine ("Sending data.");
				Thread.Sleep (5000);
			}

		}

		public static void sendCallback (IAsyncResult asyncSend)
		{
			Socket clientSocket = (Socket)asyncSend.AsyncState;
			int bytesSent = clientSocket.EndSend (asyncSend);
			System.Diagnostics.Debug.WriteLine (" bytes sent.", bytesSent.ToString ());

			StateObject stateObject = new StateObject (16, clientSocket);

		}

	}
}

