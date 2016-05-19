using System;
using System.Collections.Generic;
using GUC_Attendance.Models;

using Xamarin.Forms;
using System.Collections;
using Org.BouncyCastle.Utilities;
using XLabs.Forms.Controls;
using System.Diagnostics;
using Acr.UserDialogs;

namespace GUC_Attendance
{
	public partial class InstructorTutorialPage2 : ContentPage
	{
		SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager sqlapimanager;
		string room = "";

		public InstructorTutorialPage2 (SQLDatabase db, IEnumerable<WeeklyAttendance> w, int w_no, string datenow)
		{
			this._database = db;
			this.sqlapimanager = new SQL_API_Manager (_database);



			_data = new ListView ();

			_data.BackgroundColor = Color.FromHex ("#dbedf2");
			_data.HasUnevenRows = true;
			_data.ItemsSource = w;
			_data.ItemTemplate = new DataTemplate (typeof(InstructorTutorialCustomCell));

			Label today = new Label { Text = datenow, XAlign = TextAlignment.Center, TextColor = Color.Black };
			Label week = new Label {
				Text = "Week " + w_no,
				XAlign = TextAlignment.Center,
				TextColor = Color.FromHex ("#f35e20")
			};
					



			InitializeComponent ();

			foreach (var a in w) {
				this.title.Text = a.course;
				this.Title = a.course;
				this.room = a.room;
				break;
			}





			title.TextColor = Color.Black;

			stack.Children.Add (week);
			stack.Children.Add (today);
			stack.Children.Add (_data);





		}




	}
}

