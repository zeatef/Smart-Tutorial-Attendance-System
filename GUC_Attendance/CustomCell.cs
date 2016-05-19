// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using Xamarin.Forms;


namespace GUC_Attendance
{
	public class CustomCell : ViewCell
	{
		public CustomCell()
		{
			//instantiate each of our views
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();
			Label student = new Label ();
			Label course = new Label ();
			Label instructor = new Label ();
			Label room = new Label ();
			Label slot = new Label ();


			//set bindings
			student.SetBinding (Label.TextProperty, "student");
			course.SetBinding (Label.TextProperty, "course");
			instructor.SetBinding (Label.TextProperty, "instructor");
			room.SetBinding (Label.TextProperty, "room");
			slot.SetBinding (Label.TextProperty, "slot");



			//Set properties for desired design
			cellWrapper.BackgroundColor = Color.FromHex ("#eee");
			cellWrapper.Padding = 10;
			horizontalLayout.Orientation = StackOrientation.Vertical;
			student.TextColor = Color.FromHex ("#f35e20");
			room.TextColor = Color.FromHex ("503026");

			//add views to the view hierarchy
			horizontalLayout.Children.Add (student);
			horizontalLayout.Children.Add (course);
			horizontalLayout.Children.Add (instructor);
			horizontalLayout.Children.Add (room);
			horizontalLayout.Children.Add (slot);
			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}
	}
}

