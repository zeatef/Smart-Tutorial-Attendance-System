// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using Xamarin.Forms;
using GUC_Attendance.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using Acr.UserDialogs;
using Splat;
using XLabs.Forms;
using XLabs.Forms.Controls;
using System.Net.NetworkInformation;
using System.Net;
using System.Dynamic;


namespace GUC_Attendance
{
	public class Login:ContentPage
	{
		
		SQL_API_Manager sqlapimanager;
		SQLDatabase _database;
		string emailentry;
		string passwordentry;

		Entry email = new Entry { Text = "lamia.elbadrawy@guc.edu.eg", Placeholder = "GUC Email" };
		Entry password = new Entry {
			Text = "12345678",
			Placeholder = "Password",
			IsPassword = true
		};

		XLabs.Forms.Controls.CheckBox rememberme = new CheckBox () {
			CheckedText = "Remember Me",
			DefaultText = "Remember Me",
		};

		public Login (SQLDatabase sqldatabase)
		{
			this._database = sqldatabase;
			if (_database.CredentialsEmpty () > 0) {
				Credentials c = _database.GetCredentials ();
				email.Text = c.email;
				password.Text = c.password;
				rememberme.Checked = true;
			}
			sqlapimanager = new SQL_API_Manager (_database);

			if (Device.OS == TargetPlatform.Android) {
				email.TextColor = Color.Black;
				password.TextColor = Color.Black;
				rememberme.TextColor = Color.Black;
			}

			Title = "Login";
			BackgroundColor = Color.FromHex ("#dbedf2");

			Label title = new Label {
				Text = "GUC Attendance",
				FontAttributes = FontAttributes.Bold,
				XAlign = Xamarin.Forms.TextAlignment.Center,
				TextColor = Color.Black
			}; 

			Button loginbutton = new Button { Text = "Login", FontAttributes = FontAttributes.Bold, TextColor = Color.Green };
			loginbutton.Clicked += OnLoginClicked;
			if (Device.OS == TargetPlatform.Android) {
				loginbutton.TextColor = Color.White;
			}
			Label label = new Label {
				Text = "Don't have an account? Register Now.",
				XAlign = TextAlignment.Center,
				FontSize = 12,
				TextColor = Color.Black
			};
			Button registerbutton = new Button {
				Text = "Create Account",
				Command = new Command (() => Navigation.PushAsync (new Signup (sqldatabase))),
				FontAttributes = FontAttributes.Bold
			};
//			DateTime now = DateTime.Now.ToLocalTime();
//			Debug.WriteLine (now.Month.ToString ());
//			string currentTime = (string.Format ("Current Time: {0}", now));
//			Debug.WriteLine (currentTime);

			Content = new StackLayout {
				Spacing = 20,
				Padding = new Thickness (20),
				Children = { title, email, password, rememberme, loginbutton, label, registerbutton },
			};
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			UserDialogs.Instance.HideLoading ();
//			UserDialogs.Instance.ShowLoading ("Updating Weeks");
//			await sqlapimanager.UpdateSlotLamia ();
//			await sqlapimanager.UpdateSlotLamia2 ();
//			await sqlapimanager.UpdateSlotLamia3 ();
//
//			UserDialogs.Instance.HideLoading ();
		}

		public async void OnLoginClicked (object sender, EventArgs e)
		{
			try {
				if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
					UserDialogs.Instance.ShowLoading ("Please Wait...");
					await sqlapimanager.fetchDataFromAPItoSQL ();
					UserDialogs.Instance.HideLoading ();
				} 
				emailentry = email.Text.ToLower ();
				passwordentry = password.Text;
				if (_database.MemberExists (emailentry)) {
					if (_database.PasswordCorrect (emailentry, passwordentry)) {
						if (_database.GetMemberPosition (emailentry).Equals ("Instructor")) {
							Instructor ins = _database.GetInstructorByEmail (emailentry);
							if (rememberme.Checked) {
								_database.SaveCredentials (emailentry, passwordentry);
							} else {
								_database.ClearCredentials ();
							}
							if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
								UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and Refresh to view updated data.", 6000);
							} 
							await Navigation.PushAsync (new GUC_Attendance.MainPageInstructor (_database, ins));
						} else {
							if (rememberme.Checked) {
								_database.SaveCredentials (emailentry, passwordentry);
							} else {
								_database.ClearCredentials ();
							}
							if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
								UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and Refresh to view updated data.", 6000);
							}
							Student stu = _database.GetStudentByEmail (emailentry);
							await Navigation.PushAsync (new GUC_Attendance.MainPageStudent (_database, stu));
						}
					} else {
						if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
							UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and try again.", 6000);
						} 
						await UserDialogs.Instance.AlertAsync ("Incorrect Password", "");
					}
				} else {
					if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
						UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and try again.", 6000);
					} 
					await UserDialogs.Instance.AlertAsync ("Invalid Email Address", "");
				}
			} catch (Exception ee) {
				UserDialogs.Instance.HideLoading ();
				await UserDialogs.Instance.AlertAsync ("Please Try Again", "Network Error");
//				await UserDialogs.Instance.AlertAsync ("Please Try Again", ee.Message);

			}
		}
			
	}
}

