using System;
using Xamarin.Forms;

namespace GUC_Attendance
{
	public class Member
	{
		public string position;
		public string fname;
		public string lname;
		public string email;
		public string password;
		public string id;

		public Member (string position, string fname, string lname, string email, string password)
		{
			this.position = position;
			this.fname = fname;
			this.lname = lname;
			this.email = email;
			this.password = password;
		}

		public Member (string position, string fname, string lname, string email, string password, string id)
		{
			this.position = position;
			this.fname = fname;
			this.lname = lname;
			this.email = email;
			this.password = password;
			this.id = id;
		}

		public Member ()
		{
			
		}


	}
}

