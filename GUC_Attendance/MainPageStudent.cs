using System;
using GUC_Attendance.Models;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace GUC_Attendance
{
	public class MainPageStudent : TabbedPage
	{
		private SQLDatabase _database;
		SQL_API_Manager sqlapimanager;
		Student user;

		public MainPageStudent (SQLDatabase database, Student stu)
		{
			this.user = stu;
			this._database = database;
			sqlapimanager = new SQL_API_Manager (_database);
			NavigationPage.SetHasBackButton (this, false);

			this.Title = "Home";

			var mycourses = new Home_Student (database, user);
			mycourses.Title = "My Classes";
			mycourses.Icon = "Schedule.png";

			var today = new TodayStudent (database, user);
			today.Title = "Today";
			today.Icon = "today.png";

			Children.Add (today);
			Children.Add (mycourses);

		}

	}
}

