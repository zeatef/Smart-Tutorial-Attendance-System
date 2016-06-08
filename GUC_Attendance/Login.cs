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

		Entry email = new Entry { Placeholder = "GUC Email" };
		Entry password = new Entry {
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
				FontAttributes = FontAttributes.Bold
			};

			registerbutton.Clicked += OnRegisterClicked;

			Content = new StackLayout {
				Spacing = 20,
				Padding = new Thickness (20),
				Children = { title, email, password, rememberme, loginbutton, label, registerbutton },
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			UserDialogs.Instance.HideLoading ();
		}

		public async void OnRegisterClicked (object sender, EventArgs e)
		{
			if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
				Navigation.PushAsync (new Signup (_database));
			} else {
				UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and try again.", 6000);
			}
		}


		public async void OnLoginClicked (object sender, EventArgs e)
		{
			try {
				if (email.Text != null) {
					if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
						UserDialogs.Instance.ShowLoading ("Please Wait...");
						await sqlapimanager.fetchDataFromAPItoSQL ();
						UserDialogs.Instance.HideLoading ();
					} 
					emailentry = email.Text.ToLower ();
					passwordentry = password.Text;
				} else {
					emailentry = "";
					passwordentry = "";
				}
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
								UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. Please connect to the internet and Refresh to view updated data.", 8000);
							}
							Student stu = _database.GetStudentByEmail (emailentry);
							await Navigation.PushAsync (new GUC_Attendance.MainPageStudent (_database, stu));
						}
					} else {
						if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
							UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. If this is your first time to use the application on this device then please connect to the internet and try again.", 6000);
						} 
						await UserDialogs.Instance.AlertAsync ("Incorrect Password", "");
					}
				} else {
					if (!DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
						UserDialogs.Instance.InfoToast ("No Internet Connection", "You are not connected to the internet. If this is your first time to use the application on this device then please connect to the internet and try again.", 6000);
					} 
					await UserDialogs.Instance.AlertAsync ("Invalid Email Address", "");
				}
			} catch (System.Net.WebException ee) {
				UserDialogs.Instance.HideLoading ();
				await UserDialogs.Instance.AlertAsync ("Please Try Again", "Network Error");
			} catch (Exception ee) {
				UserDialogs.Instance.HideLoading ();
				await UserDialogs.Instance.AlertAsync ("An Error Has Occured, Please Try Again", "Error");
			} 
		}


			
	}
}

