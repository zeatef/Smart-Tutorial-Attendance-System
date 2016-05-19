using System;
using System.Collections.Generic;
using GUC_Attendance.Models;

using Xamarin.Forms;
using System.Collections;
using Org.BouncyCastle.Utilities;
using XLabs.Forms.Controls;
using System.Diagnostics;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Threading;
using Org.BouncyCastle.Asn1.X509;


namespace GUC_Attendance
{
	public partial class InstructorTutorialPage : ContentPage
	{
		SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		string room = "";
		Button record;
		Button stoprecording;
		Button uploadattendance;
		bool recording;
		enroll_view e;
		int w_no;

		public InstructorTutorialPage (SQLDatabase db, enroll_view e, int w_no, string datenow)
		{
			this._database = db;
			this.sqlapimanager = new SQL_API_Manager (_database);
			this.w_no = w_no;
			this.e = e;
			IEnumerable<WeeklyAttendance> w = _database.GetTodayAttendance (e, w_no);
			_database.ClearTodayAttendance (e, w_no);
			_data = new ListView ();

			_data.BackgroundColor = Color.FromHex ("#dbedf2");
			_data.HasUnevenRows = true;
			_data.ItemsSource = _database.GetTodayAttendance (e, w_no);
			_data.ItemTemplate = new DataTemplate (typeof(InstructorTutorialCustomCell));
			_data.IsPullToRefreshEnabled = true;
			_data.RefreshCommand = new Command (this.Refresh);
			Label today = new Label { Text = datenow, XAlign = TextAlignment.Center, TextColor = Color.Black };
			Label week = new Label {
				Text = "Week " + w_no,
				XAlign = TextAlignment.Center,
				TextColor = Color.FromHex ("#f35e20")
			};

			record = new Button{ Text = "Record Attendance" };




			InitializeComponent ();

			foreach (var a in w) {
				this.title.Text = a.course;
				this.Title = a.course;
				this.room = a.room;
				break;
			}



			record.Clicked += OnRecordClicked;


			title.TextColor = Color.Black;

			stack.Children.Add (week);
			stack.Children.Add (today);
			stack.Children.Add (record);
			stack.Children.Add (_data);





		}

		public void Refresh ()
		{
			_data.ItemsSource = _database.GetTodayAttendance (this.e, w_no);
			Task.Delay (1000);
			_data.EndRefresh ();
		}

		public async void OnRecordClicked (object sender, EventArgs e)
		{
			if (!DependencyService.Get<IGetConnectionSSID> ().getSSID ().Equals ("GUCAttendance_" + room)) {
				await UserDialogs.Instance.AlertAsync ("You must be connected to the hotspot of the tutorials's room.", "");
			} else {
				recording = true;
				record.Text = "Recording...";
				record.IsEnabled = false;
				stoprecording = new Button { Text = "Stop Recording" };
				stoprecording.Clicked += OnStopRecordingClicked;
				stack.Children.Remove (_data);
				stack.Children.Add (stoprecording);
				stack.Children.Add (_data);
				await Task.Delay (1000);
				int ip = DependencyService.Get<IGetConnectionSSID> ().getIP ();
				DependencyService.Get<ISocketProgramming> ().SetServerSocket (_database, this.e, w_no, ip);
			}
		}

		public void OnStopRecordingClicked (object sender, EventArgs e)
		{
			recording = false;
			_data.ItemsSource = _database.GetTodayAttendance (this.e, w_no);
			uploadattendance = new Button { Text = "Upload Attendance" };
			uploadattendance.Clicked += OnUploadAttendanceClicked;
			stack.Children.Remove (record);
			stack.Children.Remove (_data);
			stack.Children.Remove (stoprecording);
			stack.Children.Add (uploadattendance);
			stack.Children.Add (_data);

			DependencyService.Get<ISocketProgramming> ().CloseServerSocket ();
		}

		public async void OnUploadAttendanceClicked (object sender, EventArgs e)
		{
			try {
				UserDialogs.Instance.ShowLoading ("Uploading Data, Please Wait...");
				await sqlapimanager.UpdateTodayAttendance (this.e, this.w_no);
				UserDialogs.Instance.HideLoading ();
				UserDialogs.Instance.SuccessToast ("Attendance Uploaded Successfully");
				await Navigation.PopAsync ();

			} catch (Exception exc) {
				UserDialogs.Instance.HideLoading ();
				UserDialogs.Instance.Alert ("Slow Internet Connection - Please Try Again");
			}

		}
	}
}

