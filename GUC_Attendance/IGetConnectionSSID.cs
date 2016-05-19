using System;

namespace GUC_Attendance
{
	public interface IGetConnectionSSID
	{
		string getSSID ();

		int getIP ();

		bool IsConnectedToInternet ();
	}
}

