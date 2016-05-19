// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using Xamarin.Forms;


namespace GUC_Attendance
{
	public class InstructorCustomCell : ViewCell
	{
		public InstructorCustomCell ()
		{
			//instantiate each of our views
			StackLayout cellWrapper = new StackLayout ();
			StackLayout horizontalLayout = new StackLayout ();

			Label course = new Label ();
			Label slot = new Label ();
			Label room = new Label ();
			Label tutorial = new Label ();



			//set bindings
			course.SetBinding (Label.TextProperty, "course");
			slot.SetBinding (Label.TextProperty, "slot");
			room.SetBinding (Label.TextProperty, "room");
			tutorial.SetBinding (Label.TextProperty, "tutorial");



			//Set properties for desired design
			cellWrapper.BackgroundColor = Color.FromHex ("#dbedf2");
			cellWrapper.Padding = 10;
			horizontalLayout.Orientation = StackOrientation.Vertical;
			course.TextColor = Color.FromHex ("#f35e20");
			slot.TextColor = Color.Black;
			room.TextColor = Color.Black;
			tutorial.TextColor = Color.Black;


			//add views to the view hierarchy
			horizontalLayout.Children.Add (course);
			horizontalLayout.Children.Add (slot);
			horizontalLayout.Children.Add (room);
			horizontalLayout.Children.Add (tutorial);

			cellWrapper.Children.Add (horizontalLayout);
			View = cellWrapper;
		}
	}
}

