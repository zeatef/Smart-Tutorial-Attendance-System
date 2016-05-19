using System;
using Xamarin.Forms;
using GUC_Attendance.iOS;
using GUC_Attendance;
using Foundation;
using SystemConfiguration;
using System.Diagnostics;
using Plugin.Connectivity;





[assembly: Dependency (typeof(GetConnectionSSID_iOS))]

namespace GUC_Attendance.iOS
{
	public class GetConnectionSSID_iOS : IGetConnectionSSID
	{
		public GetConnectionSSID_iOS ()
		{
		}

		public string getSSID ()
		{
			return "Zodiac";
		}

		public int getIP ()
		{
			return 0;
		}

		public bool IsConnectedToInternet ()
		{
			return !(Reachability.InternetConnectionStatus ().ToString ().Equals ("NotReachable"));

		}
	}
}