using System;
using GUC_Attendance.Models;
using Xamarin.Forms;
using Org.BouncyCastle.Utilities.Zlib;

namespace GUC_Attendance
{
	public class MainPageInstructor : TabbedPage
	{
		private SQLDatabase _database;
		SQL_API_Manager sqlapimanager;
		Instructor user;

		public MainPageInstructor (SQLDatabase database, Instructor ins)
		{
			this.user = ins;
			this._database = database;
			sqlapimanager = new SQL_API_Manager (_database);
			NavigationPage.SetHasBackButton (this, false);

			this.Title = "Home";

			var mycourses = new Home_Instructor (database, user);
			mycourses.Title = "My Classes";
			mycourses.Icon = "Schedule.png";

			var today = new TodayInstructor (database, user);
			today.Title = "Today";
			today.Icon = "today.png";

			Children.Add (today);
			Children.Add (mycourses);

		}
	}
}

