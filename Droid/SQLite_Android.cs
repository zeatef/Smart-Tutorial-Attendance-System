using System;
using System.IO;
using Xamarin.Forms;
using GUC_Attendance.Droid;
using System.Diagnostics;
using SQLite.Net;

[assembly: Dependency (typeof(SQLite_Android))]

namespace GUC_Attendance.Droid
{
	public class SQLite_Android: ISQLite
	{
		public SQLite_Android ()
		{
		}

		#region ISQLite implementation

		public SQLite.Net.SQLiteConnection GetConnection ()
		{
			Debug.WriteLine ("Creating database, if it doesn't already exist");

			var fileName = "GUCAttendance.db3";
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var path = Path.Combine (documentsPath, fileName);

			Debug.WriteLine (path);



			var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid ();
			var connection = new SQLite.Net.SQLiteConnection (platform, path);

			return connection;
		}

		#endregion
	}
}