// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using GUC_Attendance.Models;
using Acr.UserDialogs;
using Splat;
using GUC_Attendance;
using Plugin.Connectivity;

namespace GUC_Attendance
{
	public partial class TodayStudent : ContentPage
	{
		private SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		Student user;
		string weekday;

		public TodayStudent (SQLDatabase database, Student stu)
		{
			this.user = stu;
			this._database = database;
			this.sqlapimanager = new SQL_API_Manager (_database);
			_data = new ListView ();

			_data.IsPullToRefreshEnabled = true;
			_data.RefreshCommand = new Command (this.Refresh);
			InitializeComponent ();

			NavigationPage.SetHasBackButton (this, false);
			title.Text = _database.GetStudentName (user.sid);

			DateTime now = DateTime.Now;
			int day = now.Day;
			int month = now.Month;
			int year = now.Year;
			weekday = now.DayOfWeek.ToString ();
			int subtraction = 0;
			if (weekday.Equals ("Sunday")) {
				subtraction = -1;
			} else if (weekday.Equals ("Monday")) {
				subtraction = -2;
			} else if (weekday.Equals ("Tuesday")) {
				subtraction = -3;
			} else if (weekday.Equals ("Wednesday")) {
				subtraction = -4;
			} else if (weekday.Equals ("Thursday")) {
				subtraction = -5;
			}
			string datenow = weekday + ", " + now.Day.ToString () + "/" + now.Month.ToString () + "/" + now.Year.ToString ();

			DateTime check = new DateTime (year, month, day).AddDays (subtraction);

			Debug.WriteLine (DependencyService.Get<IGetConnectionSSID> ().getSSID ());


			int w_no = 0;
			foreach (var b in _database.GetWeeks()) {
				string[] checkdate = b.start.Split ('/');
				int checkday = Int32.Parse (checkdate [0]);
				int checkmonth = Int32.Parse (checkdate [1]);
				int checkyear = Int32.Parse (checkdate [2]);
				if (checkday == check.Day && checkmonth == check.Month && checkyear == check.Year) {
					w_no = b.week_no;
				}
			}

			if (_database.StudentTakesToday (_database.GetStudentName (user.sid), weekday)) {
				Label mycourses = new Label {
					Text = datenow + ":",
					XAlign = TextAlignment.Start,
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.Black
				};
				_data.BackgroundColor = Color.FromHex ("#dbedf2");
				_data.HasUnevenRows = true;
				_data.ItemsSource = _database.FilterStudentCoursesTodayFromEnrollView (_database.GetStudentName (user.sid), weekday);
				_data.ItemTemplate = new DataTemplate (typeof(StudentCustomCell));
				_data.ItemTapped += async (sender, e) => {
					enroll_view item = (enroll_view)e.Item;
					await Navigation.PushAsync (new GUC_Attendance.StudentTutorialPage (_database, item, w_no, datenow));
				};
				stack.Children.Add (mycourses);
				stack.Children.Add (_data);
			} else {
				Label label = new Label { Text = "You don't have any tutorials today.", XAlign = TextAlignment.Center };
				stack.Children.Add (label);
				stack.Padding = new Thickness (30);
				stack.Spacing = 20;
			}
		}

		public async void Logout (object sender, EventArgs e)
		{
			int c = Navigation.NavigationStack.Count;
			foreach (var a in Navigation.NavigationStack) {
				UserDialogs.Instance.ShowLoading ("Logging Out");

				Navigation.PopAsync ();
			}
		}

		public async void Refresh ()
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					await sqlapimanager.fetchDataFromAPItoSQL ();
					_data.EndRefresh ();
					_data.ItemsSource = _database.FilterStudentCoursesTodayFromEnrollView (_database.GetStudentName (user.sid), weekday);
				} else {
					_data.EndRefresh ();
					UserDialogs.Instance.Alert ("Please connect to the internet and try refreshing again.");
				}

			} catch (System.Net.WebException ee) {
				_data.EndRefresh ();
				UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
			}

		}
	}


}

