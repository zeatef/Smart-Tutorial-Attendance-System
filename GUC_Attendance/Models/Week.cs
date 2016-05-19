using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("Week")]
	public class Week
	{
		public string id { get; set; }

		[PrimaryKey, NotNull, AutoIncrement, Unique]
		public int week_no { get; set; }

		[NotNull]
		public string start { get; set; }

		public Week ()
		{

		}
	}
}

