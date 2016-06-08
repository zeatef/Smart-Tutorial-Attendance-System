// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using CsvHelper;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Xamarin.Forms;
using GUC_Attendance.Models;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using ExifLib;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;

using Acr.UserDialogs;
using Splat;

namespace GUC_Attendance
{
	public partial class AddClass : ContentPage
	{
		bool condition;
		SQL_API_Manager sqlapimanager;
		SQLDatabase _database;
		Instructor user;

		public AddClass (SQLDatabase database, Instructor ins)
		{
			this._database = database;
			sqlapimanager = new SQL_API_Manager (_database);
			user = ins;
			InitializeComponent ();
		
			if (Device.OS == TargetPlatform.Android) {
				addclass.TextColor = Color.White;
			}

			this.SetDisplay ();
		}

		public void SetDisplay ()
		{
			addclass.Command = new Command (this.Add);
			addclass.IsEnabled = false;
			day.SelectedIndexChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			slot.SelectedIndexChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			cname.TextChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			ccode.TextChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			filename.TextChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			tutorialgroup.TextChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};

			room.TextChanged += (sender, args) => {
				condition = ((day.SelectedIndex != -1) && cnameValidator.IsValid && ccodeValidator.IsValid && (slot.SelectedIndex != -1) && filenameValidator.IsValid && roomValidator.IsValid && tutorialGroupValidator.IsValid);
				if (condition) {
					addclass.IsEnabled = true;
				} else {
					addclass.IsEnabled = false;
				}
			};


		}

		public async void Add ()
		{
			if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
				string url = "http://localhost/gucattendance/uploads/" + filename.Text + ".csv";
				Task<string> result = this.GetCSVFile (url);
				string r = await result;
				StringReader rr = new StringReader (r);
				var csv = new CsvReader (rr);
				int eid = await sqlapimanager.GetEnrollNumberOfRows ();
				int wid = await sqlapimanager.GetWeeklyAttendanceNumberOfRows ();
				int count = await sqlapimanager.GetStudentsNumberOfRows ();
				using (var dlg = UserDialogs.Instance.Progress ("Please Wait, This May Take A Few Minutes...")) {
					try {
						while (dlg.PercentComplete < 100) {
							while (csv.Read ()) {
								string id = csv.GetField<string> ("UniqAppNo");
								string fullname = csv.GetField<string> ("Fullname");
								string[] sArray = fullname.Split (' ');
								string ffname = sArray [0];
								string llname = sArray [sArray.Length - 1];
								int slot_no = sqlapimanager.stringToSlotNumber (this.getDay (day.SelectedIndex), this.getSlot (slot.SelectedIndex));

								if (!(await sqlapimanager.StudentExists (id))) {
									count++;
									var s = new Student {
										count = count,
										sid = id,
										fname = ffname,
										lname = llname
									};
									await sqlapimanager.AddStudent (s);
								}
								if (!sqlapimanager.CourseExists (ccode.Text)) {
									var s = new Course {
										name = ToUpperFirstLetter (cname.Text),
										code = ccode.Text,
									};
									await sqlapimanager.AddCourse (s);
								}
								if (!(await sqlapimanager.EnrollExists (_database.GetCourseID (ccode.Text), id, user.tid, slot_no))) {
									eid++;
									var e = new enroll { 
										eid = eid,
										sid = id,
										tid = user.tid,
										cid = _database.GetCourseID (ccode.Text),
										room = room.Text,
										slot_no = slot_no,
										tutorial = tutorialgroup.Text
									};
									await sqlapimanager.AddEnroll (e);

									var tv = new enroll_view {
										eid = eid,
										student = await sqlapimanager.GetStudentName (id),
										course = await sqlapimanager.GetCourseNameAndCode (_database.GetCourseID (ccode.Text)),
										instructor = await sqlapimanager.GetInstructorName (user.tid),
										room = room.Text,
										slot = await sqlapimanager.GetSlotName (slot_no),
										slot_no = slot_no,
										tutorial = tutorialgroup.Text

									};

									await sqlapimanager.AddEnrollView (tv);

									int w_no = 1;
									for (int i = 0; i < 12; i++) {
										wid++;
										var w = new WeeklyAttendance {
											eid = eid,
											wid = wid,
											week = w_no,
											student = await sqlapimanager.GetStudentName (id),
											course = await sqlapimanager.GetCourseNameAndCode (_database.GetCourseID (ccode.Text)),
											instructor = await sqlapimanager.GetInstructorName (user.tid),
											room = room.Text,
											slot = await sqlapimanager.GetSlotName (slot_no),
											attended = "Absent",
											slot_no = slot_no,
											tutorial = tutorialgroup.Text
										};
										await sqlapimanager.AddWeeklyAttendance (w);
										w_no++;
									}

								}
								dlg.PercentComplete += 2;
							}
							dlg.PercentComplete = 100;
						}
						UserDialogs.Instance.SuccessToast ("Success", "Class Added Successfully");
						if (DependencyService.Get<IGetConnectionSSID> ().IsConnectedToInternet ()) {
							UserDialogs.Instance.ShowLoading ("Refreshing, Please Wait...");
							await sqlapimanager.fetchDataFromAPItoSQL ();
							UserDialogs.Instance.HideLoading ();
						} else {
							UserDialogs.Instance.Alert ("Please connect to the internet and refresh.");
						}
						await Navigation.PushAsync (new GUC_Attendance.Home_Instructor (_database, user));
					} catch (System.Net.WebException ee) {
						dlg.PercentComplete = 100;
						UserDialogs.Instance.ErrorToast ("Network Error", "Please Try Again", 3000);
					}
				}
			} else {
				UserDialogs.Instance.Alert ("Please connect to the internet and try again.");
			}
		}


		public string getDay (int i)
		{
			switch (i) {
			case 0:
				return "Saturday";
			case 1:
				return "Sunday";
			case 2:
				return "Monday";
			case 3:
				return "Tuesday";
			case 4:
				return "Wednesday";
			case 5:
				return "Thursday";
			}
			return null;
		}

		public string getSlot (int i)
		{
			switch (i) {
			case 0:
				return "1st";
			case 1:
				return "2nd";
			case 2:
				return "3rd";
			case 3:
				return "4th";
			case 4:
				return "5th";
			}
			return null;
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

		public async Task<string> GetCSVFile (string url)
		{
			Uri uri = new Uri (url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (uri);
			string received;

			using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync (request.BeginGetResponse, request.EndGetResponse, null))) {
				using (var responseStream = response.GetResponseStream ()) {
					using (var sr = new StreamReader (responseStream)) {

						received = await sr.ReadToEndAsync ();
					}
				}
			}

			return received;
		}
	}
}

