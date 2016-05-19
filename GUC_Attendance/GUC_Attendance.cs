// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using Acr.UserDialogs;
using Splat;
using Xamarin.Forms;

namespace GUC_Attendance
{
	public class App : Application
	{
		SQLDatabase database;
		SQL_API_Manager manager;

		public App ()
		{
			database = new SQLDatabase ();
			manager = new SQL_API_Manager (database);
			// The root page of your application
			MainPage = new NavigationPage (new GUC_Attendance.Login (database));

		}

		public async void FetchData ()
		{
			UserDialogs.Instance.ShowLoading ("Fetching Data, Please Wait...");
			await manager.fetchDataFromAPItoSQL ();
			UserDialogs.Instance.HideLoading ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
//			this.FetchData ();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}


	}
}

