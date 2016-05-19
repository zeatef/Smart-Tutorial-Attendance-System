using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("Student")]
	public class Student
	{
		public string id { get; set; }

		[PrimaryKey, NotNull, Unique]
		public string sid { get; set; }

		public int count { get; set; }

		public string email { get; set; }

		public string password { get; set; }

		public string fname { get; set; }

		public string lname { get; set; }

		public Student ()
		{

		}

	}
}

