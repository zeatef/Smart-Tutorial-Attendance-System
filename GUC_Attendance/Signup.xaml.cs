// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Acr.UserDialogs;
using Xamarin.Forms;
using Splat;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GUC_Attendance.Models;
using Newtonsoft.Json;



namespace GUC_Attendance
{
	public partial class Signup : ContentPage
	{

		GUC_Attendance_Manager manager = new GUC_Attendance_Manager ();
		SQL_API_Manager sqlapimanager;
		SQLDatabase _database;

		//Fields related to email verification.
		int verificationcode;
		string returnedcode;
		Random rnd = new Random ();





		//Fields related to response from the user entries.
		string positionentry;
		string fnameentry;
		string lnameentry;
		string emailentry;
		string passwordentry;
		string identry;
		Member m;

		//Fields related to checking whether the user is a student or an instructor
		bool studentcondition;
		bool instructorcondition;
		bool condition = false;

		public Signup (SQLDatabase sqldatabase)
		{
			sqlapimanager = new SQL_API_Manager (sqldatabase);
			_database = sqldatabase;
			InitializeComponent ();

			sqlapimanager.fetchDataFromAPItoSQL ();

			d
//			fname.Text = "Lamia";
//			lname.Text = "El-Badrawy";
//			instructoremail.Text = "lamia.elbadrawy@guc.edu.eg";
//			password.Text = "123456781";

			this.SetDisplay ();


			signup.Clicked += OnSignupClicked;
		}

		public void SetDisplay ()
		{
			verify.Command = new Command (this.Verify);
			studentemailSuccessErrorText.IsVisible = false;
			verify.IsEnabled = false;
			position.SelectedIndexChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					studentemail.IsVisible = true;
					instructoremail.IsVisible = false;
					instructoremailSuccessErrorText.IsVisible = false;
					studentemailSuccessErrorText.IsVisible = true;
					id.IsVisible = true;
					idSuccessErrorText.IsVisible = true;
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					instructoremail.IsVisible = true;
					studentemail.IsVisible = false;
					instructoremailSuccessErrorText.IsVisible = true;
					studentemailSuccessErrorText.IsVisible = false;
					id.IsVisible = false;
					idSuccessErrorText.IsVisible = false;
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			fname.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			lname.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			studentemail.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			instructoremail.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			password.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};

			id.TextChanged += (sender, args) => {
				studentcondition = ((position.SelectedIndex == 0) && fnameValidator.IsValid && lnameValidator.IsValid && studentemailValidator.IsValid && passwordlengthValidator.IsValid && idValidator.IsValid);
				instructorcondition = ((position.SelectedIndex == 1) && fnameValidator.IsValid && lnameValidator.IsValid && instructoremailValidator.IsValid && passwordlengthValidator.IsValid);
				if (position.SelectedIndex == 0) {
					condition = studentcondition;
				} else if (position.SelectedIndex == 1) {
					condition = instructorcondition;
				} else {
					condition = false;
				}
				if (condition) {
					verify.IsEnabled = true;
				} else {
					verify.IsEnabled = false;
				}
			};
		}

		public void getReturnedFields (int i)
		{
			if (i == 0) {
				positionentry = "Student";
				fnameentry = ToUpperFirstLetter (fname.Text);
				lnameentry = ToUpperFirstLetter (lname.Text);
				emailentry = studentemail.Text.ToLower ();
				passwordentry = password.Text;
				identry = id.Text;
			} else if (i == 1) {
				positionentry = "Instructor";
				fnameentry = ToUpperFirstLetter (fname.Text);
				lnameentry = ToUpperFirstLetter (lname.Text);
				emailentry = instructoremail.Text.ToLower ();
				passwordentry = password.Text;
			}
		}

		public static string ToUpperFirstLetter (string source)
		{
			source = source.ToLower ();
			string[] strings = source.Split (' ');
			string result = "";
			for (int i = 0; i < strings.Length - 1; i++) {
				char[] letters = strings [i].ToCharArray ();
				letters [0] = char.ToUpper (letters [0]);
				result += new string (letters) + " ";
			}
			char[] letterss = strings [strings.Length - 1].ToCharArray ();
			letterss [0] = char.ToUpper (letterss [0]);
			result += new string (letterss);
			return result;
		}

		public void Verify ()
		{
			getReturnedFields (position.SelectedIndex);

			verificationcode = rnd.Next (111111, 999999);
//			verificationcode = 11;

			DependencyService.Get<ISendVerificationEmail> ().sendEmail (fnameentry, lnameentry, emailentry, verificationcode);

			string checkcode = verificationcode.ToString ();

			this.triggerVerifyPrompt1 (checkcode);
		}

		public async void triggerVerifyPrompt1 (string vc)
		{
			var result = await UserDialogs.Instance.PromptAsync (new PromptConfig {
				Title = "A verification code has been sent to " + emailentry + ", please enter this code below to verify your account",
				IsCancellable = true,
				OkText = "Verify",
				CancelText = "Cancel"
			});
			this.returnedcode = result.Text;
			if (result.Ok) {
				if (returnedcode.Equals (vc)) {
					verify.Text = "Email Verified Successfully";
					verify.TextColor = Color.Green;
					if (Device.OS == TargetPlatform.Android) {
						verify.BackgroundColor = Color.FromHex ("006622");
						verify.TextColor = Color.White;
					}
					verify.Command = null;
					if (positionentry.Equals ("Instructor")) {
						m = new Member (positionentry, fnameentry, lnameentry, emailentry, passwordentry);
					} else {
						m = new Member (positionentry, fnameentry, lnameentry, emailentry, passwordentry, identry);
					}
					signup.IsEnabled = true;
				} else {
					this.triggerVerifyPrompt2 (vc);
				}
			} 
		}

		public async void triggerVerifyPrompt2 (string vc)
		{
			var r = await UserDialogs.Instance.PromptAsync (new PromptConfig {
				Title = "Incorrect verification code, please enter your verification code again",
				IsCancellable = true,
				OkText = "Verify",
				CancelText = "Cancel"
			});
			this.returnedcode = r.Text;
			if (r.Ok) {
				if (returnedcode.Equals (vc)) {
					verify.Text = "Email Verified Successfully";
					verify.TextColor = Color.Green;
					if (Device.OS == TargetPlatform.Android) {
						verify.BackgroundColor = Color.FromHex ("006622");
						verify.TextColor = Color.White;
					}
					verify.Command = null;
					if (positionentry.Equals ("Instructor")) {
						m = new Member (positionentry, fnameentry, lnameentry, emailentry, passwordentry);
					} else {
						m = new Member (positionentry, fnameentry, lnameentry, emailentry, passwordentry, identry);
					}
					signup.IsEnabled = true;
				} else {
					this.triggerVerifyPrompt2 (vc);
				}
			} 
		}

		async void OnSignupClicked (object sender, EventArgs e)
		{
			if (positionentry.Equals ("Instructor")) {
				if (sqlapimanager.InstructorExists (emailentry)) {
					UserDialogs.Instance.Alert ("The email address you have entered is already assigned to an account, please sign in using your email address and password.");
				} else {
					await sqlapimanager.AddMember (m);
					Instructor ins = _database.GetInstructorByEmail (emailentry);
					await Navigation.PushAsync (new GUC_Attendance.MainPageInstructor (_database, ins));
				}	
			} else {
				if (await sqlapimanager.StudentExists (identry)) {
					if (sqlapimanager.StudentAlreadySignedUp (identry)) {
						UserDialogs.Instance.Alert ("The email address you have entered is already assigned to an account, please sign in using your email address and password.");
					} else {
						sqlapimanager.UpdateExistingStudent (m);
						await this.SyncData ();
						Student stu = _database.GetStudentByEmail (emailentry);
						await Navigation.PushAsync (new GUC_Attendance.MainPageStudent (_database, stu));
					}
				} else {
					await sqlapimanager.AddMember (m);
					Student stu = _database.GetStudentByEmail (emailentry);
					await Navigation.PushAsync (new GUC_Attendance.MainPageStudent (_database, stu));
				}
			}

		}

		public async Task SyncData ()
		{
			UserDialogs.Instance.ShowLoading ("Please Wait...");
			await sqlapimanager.fetchDataFromAPItoSQL ();
			UserDialogs.Instance.HideLoading ();
		}
	}
}

