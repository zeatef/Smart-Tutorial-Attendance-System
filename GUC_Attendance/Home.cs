// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using GUC_Attendance.Models;

namespace GUC_Attendance
{
	public class Home: ContentPage {
		private SQLDatabase _database;
		private ListView _data;
		SQL_API_Manager manager;
		Instructor user;

		public Home (SQLDatabase database, Instructor ins)
		{
			NavigationPage.SetHasBackButton(this, false);
			this.user = ins;
			_database = database;
			manager = new SQL_API_Manager (_database);
			manager.fetchDataFromAPItoSQL ();

			if (_database.InstructorTeaches (user.tid)) {
			
			} else {
				
			}

//			var result = _database.GetEnrolledStudentsView ();
//
//			var toolbarItem = new ToolbarItem {
//				Name = "Add",
//				Command = new Command(() => Navigation.PushAsync(new Secondary(this, database)))
//			};
//
//			ToolbarItems.Add (toolbarItem);
//
//
//			manager = new GUC_Attendance_Manager ();
//
//			Title = "My Tutorials";
//
//			_data = new ListView ();
//			_data.HasUnevenRows = true;
//			_data.ItemsSource =_database.GetEnrolledStudentsView ();
//			_data.ItemTemplate = new DataTemplate (typeof(CustomCell));
//
//
//
//			Content = _data;

		}
			

		public void Refresh() {
			_data.ItemsSource = _database.GetEnrollView ();
		}
	}
}