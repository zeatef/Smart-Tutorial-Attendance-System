using System;
using System.Collections.Generic;
using GUC_Attendance.Models;

using Xamarin.Forms;
using System.Collections;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Org.BouncyCastle.Utilities;

namespace GUC_Attendance
{
	public partial class InstructorCoursePage : ContentPage
	{

		SQLDatabase _database;
		enroll_view enrollview;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		Instructor user;

		public InstructorCoursePage (SQLDatabase db, enroll_view ee, Instructor ins)
		{
			this.user = ins;
			this._database = db;
			this.enrollview = ee;
			this.sqlapimanager = new SQL_API_Manager (_database);
			InitializeComponent ();

			_data = new ListView ();
			_data.BackgroundColor = Color.FromHex ("#dbedf2");
			_data.HasUnevenRows = true;

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
			IEnumerable<Week> dd = _database.GetWeeks ();

			List<CourseAttendanceWeekly> zodiac = new List<CourseAttendanceWeekly> ();

			foreach (var b in dd) {
				string w = "Week " + b.week_no;
				int w_no = b.week_no;
				string[] date = _database.GetWeek (w_no).Split ('/');
				int day = Int32.Parse (date [0]);
				int month = Int32.Parse (date [1]);
				int year = Int32.Parse (date [2]);
				DateTime check = new DateTime (year, month, day).AddDays (addition);
				string d = check.DayOfWeek.ToString () + ", " + check.Day.ToString () + "/" + check.Month.ToString () + "/" + check.Year.ToString ();
				CourseAttendanceWeekly ccc = new CourseAttendanceWeekly {
					week = w,
					day = d,
				};
				zodiac.Add (ccc);
			}

			_data.ItemsSource = zodiac;
			_data.ItemTemplate = new DataTemplate (typeof(InstructorCourseCustomCell));
			_data.ItemTapped += async (sender, e) => {
				CourseAttendanceWeekly item = (CourseAttendanceWeekly)e.Item;
				string[] s = item.week.Split (' ');
				int w_no = Int32.Parse (s [1]);
				IEnumerable<WeeklyAttendance> w = _database.GetTodayAttendance (enrollview, w_no);

				await Navigation.PushAsync (new GUC_Attendance.InstructorTutorialPage2 (_database, w, w_no, item.day));
			};

			Label tutorialgroup = new Label {
				Text = enrollview.tutorial,
				TextColor = Color.Black, 
				FontAttributes = FontAttributes.Bold,
				XAlign = TextAlignment.Center
			};

			this.Title = "My Classes";
			this.title.Text = enrollview.course;
			stack.Children.Add (tutorialgroup);
			stack.Children.Add (_data);


		}

		public async void RefreshData ()
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					UserDialogs.Instance.ShowLoading ("Updating Data, Please Wait...");
					await sqlapimanager.fetchDataFromAPItoSQL ();
					UserDialogs.Instance.HideLoading ();
				} else {
					UserDialogs.Instance.Alert ("Please connect to the internet and refresh the page.");
				}

			} catch (System.Net.WebException ee) {
				UserDialogs.Instance.HideLoading ();
				UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Refreshing Again", 3000);
			}
		}

		async void OnClickAddStudent (object sender, EventArgs e)
		{
			await Navigation.PushAsync (new GUC_Attendance.AddStudent (_database, enrollview, user));
		}

		async void OnClickDeleteStudent (object sender, EventArgs e)
		{
			IEnumerable<WeeklyAttendance> w = _database.GetTodayAttendance (enrollview, 1);

			await Navigation.PushAsync (new GUC_Attendance.DeleteStudent (_database, w));
		}

		async void OnClickDeleteClass (object sender, EventArgs e)
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					ConfirmConfig c = new ConfirmConfig ();
					c.UseYesNo ();
					c.Message = "Are you sure you want to delete this class?";
					c.Title = "Delete Class";
					var r = await UserDialogs.Instance.ConfirmAsync (c);
					var text = (r ? "Yes" : "No");
					if (text.Equals ("Yes")) {
						UserDialogs.Instance.ShowLoading ("Deleting Class...");
						enroll abc = await sqlapimanager.apimanager.GetEnroll (enrollview.eid);
						List<int> eids = await sqlapimanager.Geteids (abc.tid, abc.cid, abc.slot_no, abc.tutorial);
						await sqlapimanager.DeleteClass (eids);
						UserDialogs.Instance.HideLoading ();
						UserDialogs.Instance.ShowLoading ("Updating Data, Please Wait...");
						await sqlapimanager.fetchDataFromAPItoSQL ();
						UserDialogs.Instance.HideLoading ();
						await Navigation.PushAsync (new MainPageInstructor (_database, user));
					}
				} else {
					UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
				}
			} catch (Exception ee) {
				UserDialogs.Instance.HideLoading ();
				await UserDialogs.Instance.AlertAsync ("Please Try Again", "Network Error");
			}
		}

		public async Task SyncData ()
		{
			UserDialogs.Instance.ShowLoading ("Please Wait...");
			await sqlapimanager.fetchDataFromAPItoSQL ();
			UserDialogs.Instance.HideLoading ();
		}

		public async void Logout (object sender, EventArgs e)
		{
			int c = Navigation.NavigationStack.Count;
			foreach (var a in Navigation.NavigationStack) {
				UserDialogs.Instance.ShowLoading ("Logging Out");

				Navigation.PopAsync ();
			}
		}
	}
}

