using System;
using SQLite.Net.Attributes;

namespace GUC_Attendance.Models
{
	[Table ("enroll")]
	public class enroll
	{
		[PrimaryKey]
		public string id { get; set; }

		[Unique]
		public int eid { get; set; }

		public string room { get; set; }

		public string sid { get; set; }

		public int tid { get; set; }

		public int cid { get; set; }

		public int slot_no { get; set; }

		public string tutorial { get; set; }


		public enroll ()
		{

		}

	}
}

