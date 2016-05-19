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

	public class ThreadWorkClient
	{

		public TcpClient client;

		public void DoWork ()
		{
			NetworkStream networkstream = client.GetStream ();
			Byte[] buffer = Encoding.ASCII.GetBytes ("28-5365");
			networkstream.Write (buffer, 0, buffer.Length);
			Thread.Sleep (500);
			while (true) {
				networkstream.Write (buffer, 0, buffer.Length);
				System.Diagnostics.Debug.WriteLine ("Zodiac");
				Thread.Sleep (5000);
			}
		}
	}

	public class Client_Android
	{
		TcpClient client;
		string id;

		public Client_Android (string id)
		{
			try {
				this.id = id;
				IPAddress ipaddress = new IPAddress (101323948);
				System.Diagnostics.Debug.WriteLine (ipaddress.ToString ());
				client = new TcpClient ();
				this.Connect ();
			} catch (Exception e) {
				e.Message.ToString ();
			}
		}

		public void Connect ()
		{
			
			IPAddress ipaddress = new IPAddress (101323948);
			IPEndPoint endpoint = new IPEndPoint (ipaddress, 4444);
			client = new TcpClient ();
			client.Connect (ipaddress, 1994);
			System.Diagnostics.Debug.WriteLine (client.Connected);
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
	//	class StateObject
	//	{
	//		internal byte[] sBuffer;
	//		internal Socket sSocket;
	//
	//		internal StateObject (int size, Socket sock)
	//		{
	//			sBuffer = new byte[size];
	//			sSocket = sock;
	//		}
	//	}
	//
	//	public class Client_Android
	//	{
	//
	//		Socket client;
	//
	//		public Client_Android ()
	//		{
	//			IPAddress ipaddress = new IPAddress (67769516);
	//			IPEndPoint endpoint = new IPEndPoint (ipaddress, 6000);
	//			client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	//
	//			IAsyncResult asyncConnect = client.BeginConnect (endpoint, new AsyncCallback (connectCallback), client);
	//		}
	//
	//		public void connectCallback (IAsyncResult asyncConnect)
	//		{
	//			Socket clientSocket = (Socket)asyncConnect.AsyncState;
	//			clientSocket.EndConnect (asyncConnect);
	//			// arriving here means the operation completed
	//			// (asyncConnect.IsCompleted = true) but not
	//			// necessarily successfully
	//
	//			if (clientSocket.Connected == false) {
	//				System.Diagnostics.Debug.WriteLine (".client is not connected.");
	//				return;
	//			} else
	//				System.Diagnostics.Debug.WriteLine (".client is connected.");
	//
	//			byte[] sendBuffer = Encoding.ASCII.GetBytes ("Ping");
	//			IAsyncResult asyncSend = clientSocket.BeginSend (
	//				                         sendBuffer,
	//				                         0,
	//				                         sendBuffer.Length,
	//				                         SocketFlags.None,
	//				                         new AsyncCallback (sendCallback),
	//				                         clientSocket);
	//
	//			System.Diagnostics.Debug.WriteLine ("Sending data.");
	//			writeDot (asyncSend);
	//		}
	//
	//		public static void sendCallback (IAsyncResult asyncSend)
	//		{
	//			Socket clientSocket = (Socket)asyncSend.AsyncState;
	//			int bytesSent = clientSocket.EndSend (asyncSend);
	//			Console.WriteLine (
	//				".{0} bytes sent.",
	//				bytesSent.ToString ());
	//
	//			StateObject stateObject =
	//				new StateObject (16, clientSocket);
	//
	//			// this call passes the StateObject because it
	//			// needs to pass the buffer as well as the socket
	//
	//
	//			System.Diagnostics.Debug.WriteLine ("Receiving response.");
	//		}
	//
	//		internal static bool writeDot (IAsyncResult ar)
	//		{
	//			int i = 0;
	//			while (ar.IsCompleted == false) {
	//				if (i++ > 20) {
	//					Console.WriteLine ("Timed out.");
	//					return false;
	//				}
	//				Console.Write (".");
	//				Thread.Sleep (100);
	//			}
	//			return true;
	//		}
	//	}
}

