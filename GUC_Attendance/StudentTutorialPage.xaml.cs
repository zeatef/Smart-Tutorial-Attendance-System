using System;
using System.Collections.Generic;
using GUC_Attendance.Models;

using Xamarin.Forms;
using System.Collections;
using Org.BouncyCastle.Utilities;
using XLabs.Forms.Controls;
using System.Diagnostics;
using Acr.UserDialogs;
using Splat;
using Org.BouncyCastle.Crypto.Engines;

namespace GUC_Attendance
{
	public partial class StudentTutorialPage : ContentPage
	{
		SQLDatabase _database;
		enroll_view enrollview;
		int w_no;
		string datenow;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		string room = "";

		public StudentTutorialPage (SQLDatabase db, enroll_view e, int w_no, string datenow)
		{
			this._database = db;
			this.enrollview = e;
			this.w_no = w_no;
			this.datenow = datenow;
			this.sqlapimanager = new SQL_API_Manager (_database);
			this.room = e.room;

			InitializeComponent ();
		
			WeeklyAttendance w = _database.GetWeeklyAttendanceByEidWid (e.eid, this.w_no);

			Label today = new Label { Text = datenow, XAlign = TextAlignment.Center, TextColor = Color.Black };
			Label week = new Label {
				Text = "Week " + w_no,
				XAlign = TextAlignment.Center,
				TextColor = Color.FromHex ("#f35e20")
			};
			Label coursename = new Label {
				Text = w.course,
				XAlign = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.Black
			};
			string[] slots = e.slot.Split (' ');
			string slotlabel = slots [1] + " Slot";
			Label slotname = new Label { Text = slotlabel, XAlign = TextAlignment.Center, TextColor = Color.Black };
			Label roomname = new Label { Text = e.room, XAlign = TextAlignment.Center, TextColor = Color.Black };


			DateTime now = DateTime.Now;
			int hournow = now.Hour;
			int minutesnow = now.Minute;
			bool running = false;
			if (slots [1].Equals ("1st")) {
				if (hournow == 8) {
					if (minutesnow >= 30) {
						running = true;
					}
				} else if (hournow == 9) {
					running = true;
				} else if (hournow == 10) {
					if (minutesnow < 30) {
						running = true;
					}
				}
			} else if (slots [1].Equals ("2nd")) {
				if (hournow == 10) {
					if (minutesnow >= 30) {
						running = true;
					}
				} else if (hournow == 11) {
					running = true;
				} else if (hournow == 12) {
					if (minutesnow < 15) {
						running = true;
					}
				}
			} else if (slots [1].Equals ("3rd")) {
				if (hournow == 12) {
					if (minutesnow >= 15) {
						running = true;
					}
				} else if (hournow == 13) {
					running = true;
				} else if (hournow == 14) {
					if (minutesnow < 15) {
						running = true;
					}
				}
			} else if (slots [1].Equals ("4th")) {
				if (hournow == 14) {
					if (minutesnow >= 15) {
						running = true;
					}
				} else if (hournow == 15) {
					running = true;
				} 
			} else if (slots [1].Equals ("5th")) {
				if (hournow == 16) {
					running = true;
				} else if (hournow == 17) {
					running = true;
				} 
			}

			if (running) {
				Button checkbox = new Button () {
					Text = "Mark Attendance"
				};
				checkbox.Clicked += OnCheckBoxClicked;

				stack.Children.Add (coursename);
				stack.Children.Add (week);
				stack.Children.Add (today);
				stack.Children.Add (slotname);
				stack.Children.Add (roomname);
				stack.Children.Add (checkbox);
			} else {
				Label checkbox = new Label {
					Text = "This tutorial is not running right now.",
					XAlign = TextAlignment.Center,
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.Black
				};
				stack.Children.Add (coursename);
				stack.Children.Add (week);
				stack.Children.Add (today);
				stack.Children.Add (slotname);
				stack.Children.Add (roomname);
				stack.Children.Add (checkbox);
			}


			this.Title = enrollview.course;






		}

		public async void OnCheckBoxClicked (object sender, EventArgs e)
		{
			Debug.WriteLine (DependencyService.Get<IGetConnectionSSID> ().getSSID ());
			Debug.WriteLine ("GUCAttendance_" + room);
			if (!DependencyService.Get<IGetConnectionSSID> ().getSSID ().Equals ("GUCAttendance_" + room)) {
				await UserDialogs.Instance.AlertAsync ("Please connect to GUCAttendance_" + room + " in your tutorial room to mark your attendance.", "");
			} else {
				DependencyService.Get<ISocketProgramming> ().SetClientSocket (_database.GetStudentID (enrollview.student));
			}
		}
	}
}

