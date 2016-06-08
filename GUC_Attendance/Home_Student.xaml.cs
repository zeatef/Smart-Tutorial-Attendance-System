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


namespace GUC_Attendance
{
	public partial class Home_Student : ContentPage
	{
		private SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		Student user;

		public Home_Student (SQLDatabase database, Student stu)
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

			if (_database.StudentTakes (user.sid)) {
				Label mycourses = new Label {
					Text = "My Courses:",
					XAlign = TextAlignment.Start,
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.Black
				};
				_data.BackgroundColor = Color.FromHex ("#dbedf2");
				_data.HasUnevenRows = true;
				_data.ItemsSource = _database.FilterStudentCoursesFromEnrollView (_database.GetStudentName (user.sid));
				_data.ItemTemplate = new DataTemplate (typeof(StudentCustomCell));
				_data.ItemTapped += async (sender, e) => {
					enroll_view item = (enroll_view)e.Item;
					await Navigation.PushAsync (new GUC_Attendance.StudentCoursePage (_database, item));
				};
				stack.Children.Add (mycourses);
				stack.Children.Add (_data);
			} else {
				Label label = new Label {
					Text = "You are currently not linked to any course.",
					XAlign = TextAlignment.Center,
					TextColor = Color.Black
				};
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

		public async void RefreshData (object sender, EventArgs e)
		{
			if (Device.OS == TargetPlatform.iOS) {
				try {
					if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
						UserDialogs.Instance.InfoToast ("Refreshing", "Syncing data, please wait...", 100000000);
						await sqlapimanager.fetchDataFromAPItoSQL ();
						UserDialogs.Instance.SuccessToast ("Success", "Data synced successfully", 3000);
						_data.ItemsSource = _database.FilterStudentCoursesFromEnrollView (_database.GetStudentName (user.sid));
					} else {
						UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
					}

				} catch (System.Net.WebException ee) {
					UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
				}
			} else {
				try {
					if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
						UserDialogs.Instance.ShowLoading ("Refreshing, Please Wait...");
						await sqlapimanager.fetchDataFromAPItoSQL ();
						_data.ItemsSource = _database.FilterStudentCoursesFromEnrollView (_database.GetStudentName (user.sid));
						UserDialogs.Instance.HideLoading ();
					} else {
						UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
					}

				} catch (System.Net.WebException ee) {
					UserDialogs.Instance.HideLoading ();
					UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
				}
			}
		}

		public async void Refresh ()
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					await sqlapimanager.fetchDataFromAPItoSQL ();
					_data.EndRefresh ();
					_data.ItemsSource = _database.FilterStudentCoursesFromEnrollView (_database.GetStudentName (user.sid));
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

