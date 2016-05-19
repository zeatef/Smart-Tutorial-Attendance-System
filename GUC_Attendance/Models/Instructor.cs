using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("Instructor")]
	public class Instructor
	{
		
		public string id { get; set; }

		[PrimaryKey, AutoIncrement, NotNull, Unique]
		public int tid { get; set; }

		[NotNull, Unique]
		public string email { get; set; }

		public string password { get; set; }

		public string fname { get; set; }

		public string lname { get; set; }

		public Instructor ()
		{

		}

	}
}

