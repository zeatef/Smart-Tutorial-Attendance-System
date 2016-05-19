using System;
using Xamarin.Forms;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace GUC_Attendance.Droid
{
	public class ServerAsync_Android
	{
		Socket server;
		Socket client;
		IPAddress ipaddress;
		bool online;

		List<Socket> clientslist = new List<Socket> ();

		public ServerAsync_Android (IPAddress ipaddress)
		{
			this.ipaddress = ipaddress;
		}

		public void StartServer ()
		{
			try {
				IPEndPoint endpoint = new IPEndPoint (ipaddress, 6000);
				server = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				server.Bind (endpoint);
				online = true;
				Debug.WriteLine ("Server is up and running!");
				this.AcceptClients ();
			} catch (Exception e) {
				e.Message.ToString ();
			}
		}

		public void AcceptClients ()
		{
			server.Listen (100);
			IAsyncResult asyncAccept = server.BeginAccept (new AsyncCallback (this.acceptCallback), server);

		}

		public void acceptCallback (IAsyncResult asyncAccept)
		{
			Socket listenSocket = (Socket)asyncAccept.AsyncState;
			Socket serverSocket = listenSocket.EndAccept (asyncAccept);

			// arriving here means the operation completed
			// (asyncAccept.IsCompleted = true) but not
			// necessarily successfully
			if (serverSocket.Connected == false) {
				Debug.WriteLine ("Server is not connected.");
				return;
			} else
				Debug.WriteLine ("Server is connected.");

			listenSocket.Close ();

			StateObject stateObject = new StateObject (16, serverSocket);

			// this call passes the StateObject because it 
			// needs to pass the buffer as well as the socket
			Debug.WriteLine ("Receiving data...");
			while (true) {
				IAsyncResult asyncReceive = serverSocket.BeginReceive (
					                            stateObject.sBuffer,
					                            0,
					                            stateObject.sBuffer.Length,
					                            SocketFlags.None,
					                            new AsyncCallback (receiveCallback),
					                            stateObject);

			}

//			CheckTimeout (asyncReceive);
		}

		public void receiveCallback (IAsyncResult asyncReceive)
		{
			StateObject stateObject = (StateObject)asyncReceive.AsyncState;
			int bytesReceived = stateObject.sSocket.EndReceive (asyncReceive);

			Debug.WriteLine ("{0} bytes received: {1}", bytesReceived.ToString (), Encoding.ASCII.GetString (stateObject.sBuffer));
		}

		public void StopServer ()
		{
			online = false;
			if (server.Connected) {
				server.Shutdown (SocketShutdown.Both);
				server.Close ();
			}
			Debug.WriteLine ("Server Stopped");
		}

		internal bool CheckTimeout (IAsyncResult ar)
		{
			int i = 0;
			while (ar.IsCompleted == false) {
				if (i++ > 40) {
					Debug.WriteLine ("Timed out.");
					return false;
				}
				Debug.Write (".");
				Thread.Sleep (500);
			}
			return true;
		}

		internal class StateObject
		{
			internal byte[] sBuffer;
			internal Socket sSocket;

			internal StateObject (int size, Socket sock)
			{
				sBuffer = new byte[size];
				sSocket = sock;
			}
		}
	}
}

