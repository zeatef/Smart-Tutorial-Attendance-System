using System;
using System.Collections.Generic;
using GUC_Attendance.Models;

using Xamarin.Forms;
using System.Collections;
using Org.BouncyCastle.Utilities;
using Acr.UserDialogs;

namespace GUC_Attendance
{
	public partial class StudentCoursePage : ContentPage
	{
		SQLDatabase _database;
		enroll_view enrollview;
		private ListView _data;
		SQL_API_Manager sqlapimanager;

		public StudentCoursePage (SQLDatabase db, enroll_view e)
		{
			this._database = db;
			this.enrollview = e;
			this.sqlapimanager = new SQL_API_Manager (_database);

			InitializeComponent ();


			_data = new ListView ();
			_data.IsPullToRefreshEnabled = true;
			_data.RefreshCommand = new Command (this.Refresh);
			_data.BackgroundColor = Color.FromHex ("#dbedf2");
			_data.HasUnevenRows = true;
			IEnumerable<WeeklyAttendance> dd = _database.GetWeeklyAttendanceByEid (enrollview.eid);
			List<CourseAttendanceWeekly> zodiac = new List<CourseAttendanceWeekly> ();
			string[] splitted = e.slot.Split (' ');
			string dayoftutorial = splitted [0];
			int addition = 0;
			if (dayoftutorial.Equals ("Sunday")) {
				addition = 1;
			} else if (dayoftutorial.Equals ("Monday")) {
				addition = 2;
			} else if (dayoftutorial.Equals ("Tuesday")) {
				addition = 3;
			} else if (dayoftutorial.Equals ("Wednesday")) {
				addition = 4;
			} else if (dayoftutorial.Equals ("Thursday")) {
				addition = 5;
			}
			foreach (var b in dd) {
				string w = "Week " + b.week;
				int w_no = b.week;
				string[] date = _database.GetWeek (w_no).Split ('/');
				int day = Int32.Parse (date [0]);
				int month = Int32.Parse (date [1]);
				int year = Int32.Parse (date [2]);
				DateTime check = new DateTime (year, month, day).AddDays (addition);
				string d = check.DayOfWeek.ToString () + ", " + check.Day.ToString () + "/" + check.Month.ToString () + "/" + check.Year.ToString ();
				CourseAttendanceWeekly ccc = new CourseAttendanceWeekly {
					week = w,
					day = d,
					attended = b.attended
				};
				zodiac.Add (ccc);
			}
			_data.ItemsSource = zodiac;
			_data.ItemTemplate = new DataTemplate (typeof(StudentCourseCustomCell));

			this.Title = enrollview.course;

			Label attendance = new Label {
				Text = "My Attendance Status:",
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.Black
			};
			stack.Children.Add (attendance);
			stack.Children.Add (_data);

		}

		public async void Refresh ()
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					await sqlapimanager.fetchDataFromAPItoSQL ();
					IEnumerable<WeeklyAttendance> dd = _database.GetWeeklyAttendanceByEid (enrollview.eid);
					List<CourseAttendanceWeekly> zodiac = new List<CourseAttendanceWeekly> ();
					string[] splitted = enrollview.slot.Split (' ');
					string dayoftutorial = splitted [0];
					int addition = 0;
					if (dayoftutorial.Equals ("Sunday")) {
						addition = 1;
					} else if (dayoftutorial.Equals ("Monday")) {
						addition = 2;
					} else if (dayoftutorial.Equals ("Tuesday")) {
						addition = 3;
					} else if (dayoftutorial.Equals ("Wednesday")) {
						addition = 4;
					} else if (dayoftutorial.Equals ("Thursday")) {
						addition = 5;
					}
					foreach (var b in dd) {
						string w = "Week " + b.week;
						int w_no = b.week;
						string[] date = _database.GetWeek (w_no).Split ('/');
						int day = Int32.Parse (date [0]);
						int month = Int32.Parse (date [1]);
						int year = Int32.Parse (date [2]);
						DateTime check = new DateTime (year, month, day).AddDays (addition);
						string d = check.DayOfWeek.ToString () + ", " + check.Day.ToString () + "/" + check.Month.ToString () + "/" + check.Year.ToString ();
						CourseAttendanceWeekly ccc = new CourseAttendanceWeekly {
							week = w,
							day = d,
							attended = b.attended
						};
						zodiac.Add (ccc);
					}
					_data.ItemsSource = zodiac;
					_data.EndRefresh ();

				} else {
					_data.EndRefresh ();
					UserDialogs.Instance.Alert ("Please connect to the internet and try refreshing again.");
				}

			} catch (System.Net.WebException) {
				_data.EndRefresh ();
				UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
			} catch (Exception) {
				_data.EndRefresh ();
				UserDialogs.Instance.ErrorToast ("Error", "Please Try Again", 3000);
			}

		}
	}
}

