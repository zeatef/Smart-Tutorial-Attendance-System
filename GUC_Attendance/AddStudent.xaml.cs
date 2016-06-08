// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Xamarin.Forms;
using GUC_Attendance.Models;
using Acr.UserDialogs;
using System.Diagnostics;

namespace GUC_Attendance
{
	public partial class AddStudent : ContentPage
	{
		SQL_API_Manager sqlapimanager;
		SQLDatabase _database;
		Instructor user;
		enroll_view enrollview;

		public AddStudent (SQLDatabase db, enroll_view ev, Instructor ins)
		{
			InitializeComponent ();
			_database = db;
			sqlapimanager = new SQL_API_Manager (_database);
			user = ins;
			enrollview = ev;
			if (Device.OS == TargetPlatform.Android) {
				addstudent.TextColor = Color.White;
			}
			bool condition;
			fname.TextChanged += (sender, args) => {
				condition = fnameValidator.IsValid && lnameValidator.IsValid && idValidator.IsValid;
				if (condition) {
					addstudent.IsEnabled = true;
				} else {
					addstudent.IsEnabled = false;
				}
			};

			lname.TextChanged += (sender, args) => {
				condition = fnameValidator.IsValid && lnameValidator.IsValid && idValidator.IsValid;
				if (condition) {
					addstudent.IsEnabled = true;
				} else {
					addstudent.IsEnabled = false;
				}
			};

			id.TextChanged += (sender, args) => {
				condition = fnameValidator.IsValid && lnameValidator.IsValid && idValidator.IsValid;
				if (condition) {
					addstudent.IsEnabled = true;
				} else {
					addstudent.IsEnabled = false;
				}
			};

			addstudent.Clicked += this.StartAdding;
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

		public async void StartAdding (object sender, EventArgs ee)
		{
			if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
				using (var dlg = UserDialogs.Instance.Progress ("Please Wait, This May Take A Few Minutes...")) {
					try {
						while (dlg.PercentComplete < 100) {
							int count = await sqlapimanager.GetStudentsNumberOfRows ();
							int eid = await sqlapimanager.GetEnrollNumberOfRows ();
							int wid = await sqlapimanager.GetWeeklyAttendanceNumberOfRows ();
							int slot_no = enrollview.slot_no;
							string cc = enrollview.course.Split (':') [0];
							Debug.WriteLine (cc);
							if (!(await sqlapimanager.StudentExists (id.Text))) {
								count++;
								var s = new Student {
									count = count,
									sid = id.Text,
									fname = ToUpperFirstLetter (fname.Text),
									lname = ToUpperFirstLetter (lname.Text)
								};
								await sqlapimanager.AddStudent (s);
							}
							dlg.PercentComplete = 25;
							if (!(await sqlapimanager.EnrollExists (_database.GetCourseID (cc), id.Text, user.tid, slot_no))) {
								eid++;
								var e = new enroll { 
									eid = eid,
									sid = id.Text,
									tid = user.tid,
									cid = _database.GetCourseID (cc),
									room = enrollview.room,
									slot_no = slot_no,
									tutorial = enrollview.tutorial
								};
								await sqlapimanager.AddEnroll (e);
								dlg.PercentComplete = 50;
								var tv = new enroll_view {
									eid = eid,
									student = await sqlapimanager.GetStudentName (id.Text),
									course = await sqlapimanager.GetCourseNameAndCode (_database.GetCourseID (cc)),
									instructor = await sqlapimanager.GetInstructorName (user.tid),
									room = enrollview.room,
									slot = await sqlapimanager.GetSlotName (slot_no),
									slot_no = slot_no,
									tutorial = enrollview.tutorial,
								};

								await sqlapimanager.AddEnrollView (tv);
								dlg.PercentComplete = 75;

								int w_no = 1;
								for (int i = 0; i < 12; i++) {
									wid++;
									var w = new WeeklyAttendance {
										eid = eid,
										wid = wid,
										week = w_no,
										student = await sqlapimanager.GetStudentName (id.Text),
										course = await sqlapimanager.GetCourseNameAndCode (_database.GetCourseID (cc)),
										instructor = await sqlapimanager.GetInstructorName (user.tid),
										room = enrollview.room,
										slot = await sqlapimanager.GetSlotName (slot_no),
										attended = "Absent",
										slot_no = slot_no,
										tutorial = enrollview.tutorial
									};
									await sqlapimanager.AddWeeklyAttendance (w);
									w_no++;
								}
								dlg.PercentComplete = 100;
								UserDialogs.Instance.ShowLoading ("Updating Data, Please Wait...");
								await sqlapimanager.fetchDataFromAPItoSQL ();
								UserDialogs.Instance.HideLoading ();
								Navigation.PopAsync ();
								UserDialogs.Instance.SuccessToast ("Student Added Successfully", ToUpperFirstLetter (fname.Text) + " " + ToUpperFirstLetter (lname.Text) + " is added to this class successfully.");
							} else {
								dlg.PercentComplete = 100;
								UserDialogs.Instance.Alert ("Student Already Enrolled In This Class");
							} 
						} 
					} catch (Exception eee) {
						dlg.PercentComplete = 100;
						UserDialogs.Instance.HideLoading ();
						await UserDialogs.Instance.AlertAsync ("Please Try Again", "Network Error");
					}
				}
			} else {
				UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
			}

		}
	}
}

