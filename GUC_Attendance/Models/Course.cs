using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("Course")]
	public class Course
	{
		
		public string id { get; set; }

		[PrimaryKey, AutoIncrement]
		public int cid { get; set; }

		public string code { get; set; }

		public string name { get; set; }

		public Course ()
		{

		}
		
	}

}

