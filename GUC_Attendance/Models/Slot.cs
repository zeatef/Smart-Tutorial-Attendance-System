using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("Slot")]
	public class Slot
	{
		public string id { get; set; }

		[PrimaryKey, NotNull, AutoIncrement, Unique]
		public int slot_no { get; set; }

		[NotNull]
		public string day { get; set; }

		[NotNull]
		public string timing { get; set; }

		public Slot ()
		{

		}
	}
}

