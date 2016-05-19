// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using Xamarin.Forms;
using GUC_Attendance.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using Acr.UserDialogs;
using Splat;


namespace GUC_Attendance
{
	public class DeleteStudentCustomCell : ViewCell
	{
		public DeleteStudentCustomCell ()
		{
			//instantiate each of our views
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();

			Label name = new Label ();


			//set bindings
			name.SetBinding (Label.TextProperty, "student");


			//Set properties for desired design
			cellWrapper.BackgroundColor = Color.FromHex ("#dbedf2");
			cellWrapper.Padding = 10;
			horizontalLayout.Orientation = StackOrientation.Vertical;
			name.TextColor = Color.Black;



			//add views to the view hierarchy
			horizontalLayout.Children.Add (name);
			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}




	}
}

