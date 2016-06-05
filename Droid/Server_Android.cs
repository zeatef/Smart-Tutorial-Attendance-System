using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using GUC_Attendance.Droid;
using System.Collections.Generic;
using System.Threading;
using Acr.UserDialogs;
using GUC_Attendance.Models;

namespace GUC_Attendance.Droid
{
	public class ThreadWork
	{
		static int counter;
		bool online;

		public TcpClient client;

		public void DoWork ()
		{
			counter = 0;
			NetworkStream networkstream = client.GetStream ();
			Byte[] buffer = new Byte [1024];
			while (true) {
				networkstream.Read (buffer, 0, 1024);
				counter++;
				Debug.WriteLine (counter);
			}

		}
	}

	public class Server_Android
	{
		SQLDatabase _database;
		enroll_view e;
		int w_no;
		TcpListener server;
		TcpClient client;
		IPAddress ipaddress;
		Thread acceptclients;
		List<Thread> threadslist = new List<Thread> ();
		Thread calcualateduration;
		bool online;
		int duration = 59;
		List<TcpClient> clientslist = new List<TcpClient> ();
		Dictionary<string, int> attendancelist = new Dictionary<string, int> ();
		IEnumerable<WeeklyAttendance> itemssource;

		public Server_Android (SQLDatabase db, enroll_view e, int w_no, IPAddress ipaddress, IEnumerable<WeeklyAttendance> itemssource)
		{
			this._database = db;
			this.e = e;
			this.w_no = w_no;
			this.ipaddress = ipaddress;
			this.itemssource = itemssource;
			Debug.WriteLine (ipaddress.ToString ());
			IPHostEntry hostEntry = Dns.GetHostEntry (ipaddress);
			string hostName = hostEntry.HostName;
			Debug.WriteLine (hostName);
		}

		public void StartServer ()
		{
			try {
				IPEndPoint endpoint = new IPEndPoint (ipaddress, 1994);
				server = new TcpListener (endpoint);
				server.Start ();
				online = true;

				calcualateduration = new Thread (
					() => {
						while (online) {
							duration++;
							Debug.WriteLine ("Duration: " + duration);
							Thread.Sleep (60000);
						}
					});
				calcualateduration.Start ();
				Debug.WriteLine ("Server is up and running!");
				acceptclients = new Thread (() => {
					this.AcceptClients ();
				});
				acceptclients.Start ();
			} catch (Exception e) {
				e.Message.ToString ();
			}
		}

		public void AcceptClients ()
		{
			while (online) {
				client = server.AcceptTcpClient ();
				Debug.WriteLine ("Accepted Client");
				clientslist.Add (client);
				var thread = new Thread (
					             () => {
						NetworkStream networkstream = client.GetStream ();
						Byte[] temp = new Byte [1024];
						int size = networkstream.Read (temp, 0, 1024);
						Byte[] buffer = new Byte [size];
						networkstream.Read (buffer, 0, buffer.Length);
						bool late = false;
						string id = Encoding.ASCII.GetString (buffer);
						if (!attendancelist.ContainsKey (id)) {
							if (duration > 15) {
								Debug.WriteLine ("Updating Attendance (Late)");
								_database.UpdateTodayAttendanceLate (e, w_no, id);
								late = true;
								attendancelist.Add (id, 0);
							} else {
								Debug.WriteLine ("Updating Attendance (Fully Attended)");
								_database.UpdateTodayAttendance (e, w_no, id);
							}
						} 
						while (online) {
							networkstream.Read (buffer, 0, buffer.Length);
							attendancelist [id]++;
							if (duration % 10 == 0) {
								if (!_database.GetTodayAttendanceStudent (e, w_no, id).Contains ("Late")) {
									if (((decimal)(attendancelist [id] / (decimal)duration) * 100m) < 75) {
										Debug.WriteLine ("Updating Attendance (Less than 75%)");
										_database.UpdateTodayAttendancePartially (e, w_no, id);
									} else {
										Debug.WriteLine ("Updating Attendance (Fully Attended)");
										_database.UpdateTodayAttendance (e, w_no, id);
									}
								} else {
									if (((decimal)(attendancelist [id] / (decimal)duration) * 100m) < 75) {
										Debug.WriteLine ("Updating Attendance (Late and less than 75%)");
										_database.UpdateTodayAttendancePartiallyLate (e, w_no, id);
									} else {
										Debug.WriteLine ("Updating Attendance (Late)");
										_database.UpdateTodayAttendanceLate (e, w_no, id);
									}
								}
							}
							Debug.WriteLine (attendancelist [id].ToString ());
						}
					});
				threadslist.Add (thread);
				thread.Start ();
			}

		}

		public void StopServer ()
		{
			online = false;
//			foreach (Thread t in threadslist) {
//				t.Join ();
//				t.Abort ();
//			}
			foreach (TcpClient c in clientslist) {
				var thread = new Thread (
					             () => {
						NetworkStream networkstream = c.GetStream ();
						Byte[] buffer = Encoding.ASCII.GetBytes ("END");
						networkstream.Write (buffer, 0, buffer.Length);
						networkstream.Close ();
						client.Close ();
					});
				thread.Start ();
			}
			acceptclients.Abort ();
			foreach (KeyValuePair<string, int> temp in attendancelist) {
				Debug.WriteLine (temp.Key + " " + temp.Value);
			}
			server.Stop ();
			calcualateduration.Join ();
			calcualateduration.Abort ();
			Debug.WriteLine ("Server Stopped");
		}

	}

	//		IPAddress ipaddress;
	//		Socket server;
	//		bool online;
	//
	//
	//
	//		internal class StateObject
	//		{
	//			internal byte[] sBuffer;
	//			internal Socket sSocket;
	//
	//			internal StateObject (int size, Socket sock)
	//			{
	//				sBuffer = new byte[size];
	//				sSocket = sock;
	//			}
	//		}
	//
	//		internal static bool writeDot (IAsyncResult ar)
	//		{
	//			int i = 0;
	//			while (ar.IsCompleted == false) {
	//				if (i++ > 40) {
	//					Console.WriteLine ("Timed out.");
	//					return false;
	//				}
	//				Console.Write (".");
	//				Thread.Sleep (500);
	//			}
	//			return true;
	//		}
	//
	//
	//		public Server_Android (IPAddress ipaddress)
	//		{
	//			this.ipaddress = ipaddress;
	//		}
	//
	//		public void StartServer ()
	//		{
	//			try {
	//				IPEndPoint endpoint = new IPEndPoint (ipaddress, 6000);
	//				server = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	//				server.Bind (endpoint);
	//				online = true;
	//				Debug.WriteLine ("Server is up and running!");
	//				this.AcceptClients ();
	//			} catch (Exception e) {
	//				e.Message.ToString ();
	//			}
	//		}
	//
	//		public void AcceptClients ()
	//		{
	//			while (online) {
	//			server.Listen (1);
	//			IAsyncResult asyncAccept = server.BeginAccept (new AsyncCallback (this.AcceptCallback), server);
	//			Debug.Write ("Connection in progress.");
	//		}
	//
	//		}
	//
	//		public void AcceptCallback (IAsyncResult asyncAccept)
	//		{
	//			Socket listenSocket = (Socket)asyncAccept.AsyncState;
	//			Socket serverSocket = listenSocket.EndAccept (asyncAccept);
	//
	//			// arriving here means the operation completed
	//			// (asyncAccept.IsCompleted = true) but not
	//			// necessarily successfully
	//			if (serverSocket.Connected == false) {
	//				Debug.WriteLine ("Server is not connected.");
	//				return;
	//			} else {
	//				Debug.WriteLine ("Server is not connected.");
	//			}
	//
	//			listenSocket.Close ();
	//
	//			StateObject stateObject = new StateObject (16, serverSocket);
	//
	//			// this call passes the StateObject because it
	//			// needs to pass the buffer as well as the socket
	//			IAsyncResult asyncReceive =
	//				serverSocket.BeginReceive (
	//					stateObject.sBuffer,
	//					0,
	//					stateObject.sBuffer.Length,
	//					SocketFlags.None,
	//					new AsyncCallback (ReceiveCallback),
	//					stateObject);
	//
	//			Debug.WriteLine ("Receiving data.");
	//			writeDot (asyncReceive);
	//		}
	//
	//		public static void ReceiveCallback (IAsyncResult asyncReceive)
	//		{
	//			StateObject stateObject = (StateObject)asyncReceive.AsyncState;
	//			int bytesReceived = stateObject.sSocket.EndReceive (asyncReceive);
	//
	//			Debug.WriteLine (".{0} bytes received: {1}", bytesReceived.ToString (), Encoding.ASCII.GetString (stateObject.sBuffer));
	//
	//		}
	//
	//		public void StopServer ()
	//		{
	//			online = false;
	//			server.Disconnect (true);
	//			Debug.WriteLine ("Server Stopped");
	//		}

}

