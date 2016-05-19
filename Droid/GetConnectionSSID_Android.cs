using System;
using Xamarin.Forms;
using System.Diagnostics;
using GUC_Attendance.Droid;
using GUC_Attendance;
using Android.Net.Wifi;
using Android.Content;
using Plugin.Connectivity;
using XLabs.Platform.Services;


[assembly: Dependency (typeof(GetConnectionSSID_Android))]

namespace GUC_Attendance.Droid
{
	public class GetConnectionSSID_Android : IGetConnectionSSID
	{
		public GetConnectionSSID_Android ()
		{
		}

		public string getSSID ()
		{
			Context c = Android.App.Application.Context;
			WifiManager wifiManager = (WifiManager)c.GetSystemService ("wifi");
			WifiInfo wifiInfo = wifiManager.ConnectionInfo;
			return wifiInfo.SSID.Trim (new Char[] { ' ', '"' }).ToString ();
		}

		public int getIP ()
		{
			Context c = Android.App.Application.Context;
			WifiManager wifiManager = (WifiManager)c.GetSystemService ("wifi");
			WifiInfo wifiInfo = wifiManager.ConnectionInfo;
			return wifiInfo.IpAddress;
		}

		public bool IsConnectedToInternet ()
		{
			return !(Reachability.InternetConnectionStatus ().ToString ().Equals ("NotReachable"));

		}

	}
}