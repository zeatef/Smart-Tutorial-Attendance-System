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
	public class StudentCustomCell : ViewCell
	{
		public StudentCustomCell ()
		{
			//instantiate each of our views
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();

			Label course = new Label ();
			Label instructor = new Label ();
			Label slot = new Label ();
			Label room = new Label ();


			//set bindings
			course.SetBinding (Label.TextProperty, "course");
			instructor.SetBinding (Label.TextProperty, "instructor");
			slot.SetBinding (Label.TextProperty, "slot");
			room.SetBinding (Label.TextProperty, "room");


			//Set properties for desired design
			cellWrapper.BackgroundColor = Color.FromHex ("#dbedf2");
			cellWrapper.Padding = 10;
			horizontalLayout.Orientation = StackOrientation.Vertical;
			course.TextColor = Color.FromHex ("#f35e20");
			instructor.TextColor = Color.Black;
			slot.TextColor = Color.Black;
			room.TextColor = Color.Black;
			//add views to the view hierarchy
			horizontalLayout.Children.Add (course);
			horizontalLayout.Children.Add (instructor);
			horizontalLayout.Children.Add (slot);
			horizontalLayout.Children.Add (room);

			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}




	}
}

