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
	public partial class Home_Instructor : ContentPage
	{
		private SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		Instructor user;

		public Home_Instructor (SQLDatabase database, Instructor ins)
		{
			this.user = ins;
			this._database = database;
			this.sqlapimanager = new SQL_API_Manager (_database);


			InitializeComponent ();
			_data = new ListView ();
			_data.IsPullToRefreshEnabled = true;
			_data.RefreshCommand = new Command (this.Refresh);

			NavigationPage.SetHasBackButton (this, false);
			title.Text = _database.GetInstructorName (ins.tid);

			if (_database.InstructorTeaches (user.tid)) {
				Button addclass = new Button { Text = "Add Another Class", FontAttributes = FontAttributes.Bold };
				addclass.Clicked += OnAddClassClicked;
				_data.BackgroundColor = Color.FromHex ("#dbedf2");
				_data.HasUnevenRows = true;
				_data.ItemsSource = _database.FilterInstuctorCoursesFromEnrollView (_database.GetInstructorName (user.tid));
				_data.ItemTemplate = new DataTemplate (typeof(InstructorCustomCell2));
				_data.ItemTapped += async (sender, e) => {
					enroll_view item = (enroll_view)e.Item;
					await Navigation.PushAsync (new GUC_Attendance.InstructorCoursePage (_database, item, user));
				};
				stack.Children.Add (addclass);
				stack.Children.Add (_data);
			} else {
				Label label = new Label { Text = "You are currently not linked to any course.", XAlign = TextAlignment.Center };
				Button addclass = new Button { Text = "Add A Class", FontAttributes = FontAttributes.Bold };
				addclass.Clicked += OnAddClassClicked;
				stack.Children.Add (label);
				stack.Children.Add (addclass);
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
						_data.ItemsSource = _database.FilterInstuctorCoursesFromEnrollView (_database.GetInstructorName (user.tid));
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
						_data.ItemsSource = _database.FilterInstuctorCoursesFromEnrollView (_database.GetInstructorName (user.tid));
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
					_data.ItemsSource = _database.FilterInstuctorCoursesFromEnrollView (_database.GetInstructorName (user.tid));
				} else {
					_data.EndRefresh ();
					UserDialogs.Instance.Alert ("Please connect to the internet and try refreshing again.");
				}

			} catch (System.Net.WebException ee) {
				_data.EndRefresh ();
				UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
			}

		}

		public void OnAddClassClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new GUC_Attendance.AddClass (_database, user));
		}
	}


}

