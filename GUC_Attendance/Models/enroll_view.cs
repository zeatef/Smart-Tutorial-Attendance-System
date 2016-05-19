using System;
using SQLite.Net.Attributes;
using Org.BouncyCastle.Math;

namespace GUC_Attendance.Models
{
	[Table ("enroll_view")]
	public class enroll_view
	{
		[PrimaryKey]
		public string id { get; set; }

		public int eid { get; set; }

		public string student { get; set; }

		public string course { get; set; }

		public string instructor { get; set; }

		public string room { get; set; }

		public string slot { get; set; }

		public int slot_no { get; set; }

		public string tutorial { get; set; }


		public enroll_view ()
		{

		}
	}
}

