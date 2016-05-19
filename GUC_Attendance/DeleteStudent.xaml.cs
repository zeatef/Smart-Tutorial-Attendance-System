using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Org.BouncyCastle.Utilities.Encoders;
using GUC_Attendance.Models;
using Acr.UserDialogs;

namespace GUC_Attendance
{
	public partial class DeleteStudent : ContentPage
	{
		IEnumerable<WeeklyAttendance> w;
		SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;


		public DeleteStudent (SQLDatabase db, IEnumerable<WeeklyAttendance> w)
		{
			this._database = db;
			this.w = w;
			InitializeComponent ();
			sqlapimanager = new SQL_API_Manager (_database);


			this.Title = "Delete Student";
			this.title.Text = "Please Choose A Student:";
			_data = new ListView ();
			_data.BackgroundColor = Color.FromHex ("#dbedf2");
			_data.HasUnevenRows = true;
			_data.ItemTemplate = new DataTemplate (typeof(DeleteStudentCustomCell));
			_data.ItemsSource = w;
			stack.Children.Add (_data);
			_data.ItemTapped += async (sender, e) => {
				try {
					WeeklyAttendance item = (WeeklyAttendance)e.Item;
					ConfirmConfig c = new ConfirmConfig ();
					c.UseYesNo ();
					c.Message = "Are you sure you want to remove " + item.student + " from this class?";
					c.Title = "Delete Student";
					var r = await UserDialogs.Instance.ConfirmAsync (c);
					var text = (r ? "Yes" : "No");
					if (text.Equals ("Yes")) {
						if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
							UserDialogs.Instance.ShowLoading ("Removing Student...");
							await sqlapimanager.RemoveStudentFromClass (item.eid);
							UserDialogs.Instance.HideLoading ();
							UserDialogs.Instance.ShowLoading ("Updating Data, Please Wait...");
							await sqlapimanager.fetchDataFromAPItoSQL ();
							UserDialogs.Instance.HideLoading ();
							await Navigation.PopAsync ();
							UserDialogs.Instance.SuccessToast ("Student Removed Successfully", item.student + " is removed from this class successfully.");
						} else {
							UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
						}
					}
				} catch (Exception ee) {
					UserDialogs.Instance.HideLoading ();
					await UserDialogs.Instance.AlertAsync ("Please Try Again", "Network Error");
				}

			};


		}
	}
}

