using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("WeeklyAttendance")]
	public class WeeklyAttendance
	{
		[PrimaryKey]
		public string id { get; set; }

		public int wid { get; set; }

		public int eid { get; set; }

		public int week { get; set; }

		public string student { get; set; }

		public string course { get; set; }

		public string instructor { get; set; }

		public string room { get; set; }

		public string slot { get; set; }

		public string attended { get; set; }

		public int slot_no { get; set; }

		public string tutorial { get; set; }


		public WeeklyAttendance ()
		{

		}
	}
}

