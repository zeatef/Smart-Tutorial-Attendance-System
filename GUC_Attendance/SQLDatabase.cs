// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using System.Linq;
using GUC_Attendance.Models;
using SQLite.Net;
using Xamarin.Forms;
using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Asn1.Pkcs;

namespace GUC_Attendance
{
	public class SQLDatabase
	{
		static object locker = new object ();
		private SQLiteConnection _connection;

		public SQLDatabase ()
		{
			_connection = DependencyService.Get<ISQLite> ().GetConnection ();
	
			_connection.CreateTable<Student> ();
			_connection.CreateTable<Instructor> ();
			_connection.CreateTable<Course> ();
			_connection.CreateTable<Slot> ();
			_connection.CreateTable<enroll> ();
			_connection.CreateTable<enroll_view> ();
			_connection.CreateTable<WeeklyAttendance> ();
			_connection.CreateTable<Week> ();
			_connection.CreateTable<Credentials> ();
		}


		//Methods on Table: Student
		//****************************************************************************************************

		public IEnumerable<Student> GetStudents ()
		{
			lock (locker) {
				return (from t in _connection.Table<Student> ()
				        select t).ToList ();
			}
		}

		public void ClearCredentials ()
		{
			_connection.DeleteAll<Credentials> ();
		}

		public void SaveCredentials (string email, string password)
		{
			_connection.DeleteAll<Credentials> ();
			_connection.Insert (new Credentials {
				email = email,
				password = password
			});
		}

		public int CredentialsEmpty ()
		{
			return _connection.Table<Credentials> ().Count ();
		}

		public Credentials GetCredentials ()
		{
			return _connection.Table<Credentials> ().Last ();
		}


		public Student GetStudentByEmail (String email)
		{
			lock (locker) {
				return _connection.Table<Student> ().Where (x => x.email.Equals (email)).First ();
			}
		}

		public String GetStudentName (String id)
		{
			lock (locker) {
				Student s = _connection.Table<Student> ().FirstOrDefault (x => x.sid == id);
				return s.fname + " " + s.lname;
			}
		}

		public String GetStudentID (String name)
		{
			lock (locker) {
				string[] ss = name.Split (' ');
				string fname = ss [0];
				string lname = ss [1];
				Student s = _connection.Table<Student> ().FirstOrDefault (x => x.fname.Equals (fname) && x.lname.Equals (lname));
				return s.sid;
			}
		}

		public void DeleteStudent (String id)
		{
			lock (locker) {
				_connection.Delete<Student> (id);
			}
		}

		public void DeleteInstructor (String id)
		{
			lock (locker) {
				_connection.Delete<Instructor> (id);
			}
		}

		public void DeleteCourse (String id)
		{
			lock (locker) {
				_connection.Delete<Course> (id);
			}
		}

		public void DeleteEnroll (String id)
		{
			lock (locker) {
				_connection.Delete<enroll> (id);
			}
		}

		public void DeleteEnrollView (String id)
		{
			lock (locker) {
				_connection.Delete<enroll_view> (id);
			}
		}

		public void DeleteWeeklyAttendance (String id)
		{
			lock (locker) {
				_connection.Delete<WeeklyAttendance> (id);
			}
		}

		public void DeleteAllStudents ()
		{
			lock (locker) {
				_connection.DeleteAll<Student> ();
			}
		}

		public void AddStudent (Student s)
		{
			lock (locker) {
				_connection.Insert (s);
			}
		}

		public void UpdateStudentEmail (string sid, string email)
		{
			lock (locker) {
				_connection.Execute ("UPDATE Student Set email  = ? WHERE sid = ?", email, sid);
			}

		}

		public void UpdateStudentPassword (string sid, string password)
		{
			lock (locker) {
				_connection.Execute ("UPDATE Student Set password  = ? WHERE sid = ?", password, sid);
			}

		}
			
		//Methods on Table: Instructor
		//****************************************************************************************************

		public IEnumerable<Instructor> GetInstructors ()
		{
			lock (locker) {
				return (from t in _connection.Table<Instructor> ()
				        select t).ToList ();
			}
		}

		public void AddInstructor (Instructor i)
		{
			lock (locker) {
				_connection.Insert (i);
			}

		}

		public void UpdateInstructorID (int tid, string id)
		{
			lock (locker) {
				_connection.Execute ("UPDATE Instructor Set id  = ? WHERE tid = ?", id, tid);
			}

		}

		public Instructor GetInstructorByEmail (string email)
		{
			lock (locker) {
				return _connection.Table<Instructor> ().FirstOrDefault (x => x.email.Equals (email));
			}
		}

		public int GetInstructorID (string email)
		{
			lock (locker) {
				return _connection.Table<Instructor> ().FirstOrDefault (x => x.email.Equals (email)).tid;
			}
		}

		public String GetInstructorName (int id)
		{
			lock (locker) {
				Instructor s = _connection.Table<Instructor> ().FirstOrDefault (x => x.tid == id);
				return s.fname + " " + s.lname;
			}
		}


		//Methods on Table: Course
		//****************************************************************************************************

		public IEnumerable<Course> GetCourses ()
		{
			lock (locker) {
				return (from t in _connection.Table<Course> ()
				        select t).ToList ();
			}
		}

		public String GetCourseNameAndCode (int id)
		{
			lock (locker) {
				Course s = _connection.Table<Course> ().FirstOrDefault (x => x.cid == id);
				return s.code + ": " + s.name;
			}
		}

		public void UpdateCourseID (int cid, string id)
		{
			lock (locker) {
				_connection.Execute ("UPDATE Course Set id  = ? WHERE cid = ?", id, cid);
			}

		}

		public int GetCourseID (string code)
		{
			lock (locker) {
				return _connection.Table<Course> ().FirstOrDefault (x => x.code.Equals (code)).cid;
			}
		}

		public void AddCourse (Course c)
		{
			lock (locker) {
				_connection.Insert (c);
			}

		}

		//Methods on Table: Slot
		//****************************************************************************************************

		public IEnumerable<Slot> GetSlots ()
		{
			lock (locker) {
				return (from t in _connection.Table<Slot> ()
				        select t).ToList ();
			}
		}

		public String GetSlotName (int id)
		{
			lock (locker) {
				Slot s = _connection.Table<Slot> ().FirstOrDefault (x => x.slot_no == id);
				return s.day + " " + s.timing;
			}
		}

		public int GetSlotNumber (string name)
		{
			lock (locker) {
				string[] splitted = name.Split (' ');
				return _connection.Table<Slot> ().FirstOrDefault (x => x.day.Equals (splitted [0]) && x.timing.Equals (splitted [1])).slot_no;
			}
		}

		public void AddSlot (Slot s)
		{
			lock (locker) {
				_connection.Insert (s);
			}

		}

		public void AddWeek (Week w)
		{
			lock (locker) {
				_connection.Insert (w);
			}

		}

		public string GetWeek (int week_no)
		{
			lock (locker) {
				return _connection.Table<Week> ().FirstOrDefault (x => x.week_no == week_no).start;
			}
		}

		public void SetSlots ()
		{
			lock (locker) {
				int check = 1;
				string d = "";
				string t = "";

				for (int i = 0; i < 6; i++) {
					switch (check) {
					case 1:
						d = "Saturday";
						break;
					case 2:
						d = "Sunday";
						break;
					case 3:
						d = "Monday";
						break;
					case 4:
						d = "Tuesday";
						break;
					case 5:
						d = "Wednesday";
						break;
					case 6:
						d = "Thursday";
						break;
					}

					for (int j = 0; j < 5; j++) {
						switch (j) {
						case 0:
							t = "1st";
							break;
						case 1:
							t = "2nd";
							break;
						case 2:
							t = "3rd";
							break;
						case 3:
							t = "4th";
							break;
						case 4:
							t = "5th";
							break;
						}
						var slot = new Slot {
							day = d,
							timing = t
						};
						_connection.Insert (slot);
					}

					check++;

				}
			}
		}

		public IEnumerable<Week> GetWeeks ()
		{
			lock (locker) {
				return (from t in _connection.Table<Week> ()
				        select t).ToList ();
			}
		}





		//Methods on Table: enroll
		//****************************************************************************************************

		public IEnumerable<enroll> GetEnroll ()
		{
			lock (locker) {
				return (from t in _connection.Table<enroll> ()
				        select t).ToList ();
			}
		}

		public void AddEnroll (enroll e)
		{
			lock (locker) {
				_connection.Insert (e);
			}

		}

		public int GetEnrollID (int slot_no, string sid)
		{
			lock (locker) {
				return _connection.Table<enroll> ().FirstOrDefault (x => x.sid.Equals (sid) && x.slot_no == slot_no).eid;
			}
		}

		public void UpdateEnrollID (int eid, string id)
		{
			lock (locker) {
				_connection.Execute ("UPDATE enroll Set id  = ? WHERE eid = ?", id, eid);
			}
		}


		//Methods on Table: enroll_view
		//****************************************************************************************************

		public IEnumerable<enroll_view> FilterInstuctorCoursesFromEnrollView (string name)
		{
			lock (locker) {
				return _connection.Query<enroll_view> ("SELECT * FROM enroll_view WHERE instructor = ? GROUP BY slot ORDER BY slot_no", name);
			}
		}

		public IEnumerable<enroll_view> FilterStudentCoursesFromEnrollView (string name)
		{
			lock (locker) {
				return _connection.Query<enroll_view> ("SELECT * FROM enroll_view WHERE student = ? GROUP BY slot ORDER BY slot_no", name);
			}
		}

		public IEnumerable<enroll_view> FilterStudentCoursesTodayFromEnrollView (string name, string day)
		{
			lock (locker) {
				day = day + "%";
				return _connection.Query<enroll_view> ("SELECT * FROM enroll_view WHERE student = ? AND slot LIKE ? GROUP BY slot ORDER BY slot_no", name, day);
			}
		}

		public IEnumerable<enroll_view> FilterInstructorCoursesTodayFromEnrollView (string name, string day)
		{
			lock (locker) {
				day = day + "%";
				return _connection.Query<enroll_view> ("SELECT * FROM enroll_view WHERE instructor = ? AND slot LIKE ? GROUP BY slot ORDER BY slot_no", name, day);
			}
		}

		public void AddEnrollView (enroll_view e)
		{
			lock (locker) {
				_connection.Insert (e);
			}

		}

		public IEnumerable<enroll_view> GetEnrollView ()
		{
			lock (locker) {
				return (from t in _connection.Table<enroll_view> ()
				        select t).ToList ();
			}
		}

		public void CreateEnrollViewTable ()
		{
			lock (locker) {
				IEnumerable<enroll> zodiac = GetEnroll ();
				foreach (enroll item in zodiac) {
					var tv = new enroll_view {
						student = this.GetStudentName (item.sid),
						course = this.GetCourseNameAndCode (item.cid),
						instructor = this.GetInstructorName (item.tid),
						room = item.room,
						slot = this.GetSlotName (item.slot_no)
					};

					_connection.Insert (tv);
				}

			}
		}

		//Methods on Table: enroll
		//****************************************************************************************************


		public void CreateWeeklyAttendance ()
		{
			lock (locker) {
				IEnumerable<enroll> zodiac = GetEnroll ();
				foreach (enroll item in zodiac) {
					int w_no = 1;
					for (int i = 0; i < 12; i++) {
						var tv = new WeeklyAttendance {
							week = w_no,
							student = this.GetStudentName (item.sid),
							course = this.GetCourseNameAndCode (item.cid),
							instructor = this.GetInstructorName (item.tid),
							room = item.room,
							slot = this.GetSlotName (item.slot_no),
							attended = "No"
						};
						_connection.Insert (tv);
						w_no++;
					}

				}

			}
		}

		public IEnumerable<WeeklyAttendance> GetWeeklyAttendance ()
		{
			lock (locker) {
				return (from t in _connection.Table<WeeklyAttendance> ()
				        select t).ToList ();
			}
		}

		public IEnumerable<WeeklyAttendance> GetWeeklyAttendanceByEid (int eid)
		{
			lock (locker) {
				return (from t in _connection.Table<WeeklyAttendance> ().Where (x => x.eid == eid)
				        select t).ToList ();
			}
		}

		public WeeklyAttendance GetWeeklyAttendanceByEidWid (int eid, int wid)
		{
			lock (locker) {
				return _connection.Table<WeeklyAttendance> ().Where (x => x.eid == eid && x.week == wid).First ();
			}
		}

		public IEnumerable<WeeklyAttendance> GetTodayAttendance (enroll_view ev, int wid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string course = ev.course;
			lock (locker) {
				return _connection.Table<WeeklyAttendance> ().Where (x => x.slot_no == slot && x.instructor.Equals (ins) && x.course.Equals (course) && x.week == wid).ToList ();
			}
		}

		public void UpdateTodayAttendance (enroll_view ev, int wid, string sid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string stu = this.GetStudentName (sid);
			string course = ev.course;
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended = 'Attended' WHERE (slot_no = ? AND instructor = ? AND course = ? AND week = ? AND student = ?)", slot, ins, course, wid, stu);
			}
		}

		public string GetTodayAttendanceStudent (enroll_view ev, int wid, string sid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string course = ev.course;
			string stu = this.GetStudentName (sid);
			lock (locker) {
				IEnumerable<WeeklyAttendance> check = _connection.Table<WeeklyAttendance> ().Where (x => x.slot_no == slot && x.instructor.Equals (ins) && x.course.Equals (course) && x.week == wid).ToList ();
				foreach (WeeklyAttendance item in check) {
					if (item.student.Equals (stu)) {
						return item.attended;
					}
				}
			}
			return null;
		}

		public void UpdateTodayAttendancePartially (enroll_view ev, int wid, string sid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string stu = this.GetStudentName (sid);
			string course = ev.course;
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended = 'Attended Less Than 75%' WHERE (slot_no = ? AND instructor = ? AND course = ? AND week = ? AND student = ?)", slot, ins, course, wid, stu);
			}
		}

		public void UpdateTodayAttendanceLate (enroll_view ev, int wid, string sid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string stu = this.GetStudentName (sid);
			string course = ev.course;
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended = 'Attended (Late)' WHERE (slot_no = ? AND instructor = ? AND course = ? AND week = ? AND student = ?)", slot, ins, course, wid, stu);
			}
		}

		public void UpdateTodayAttendancePartiallyLate (enroll_view ev, int wid, string sid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string stu = this.GetStudentName (sid);
			string course = ev.course;
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended = 'Attended Less Than 75% (Late)' WHERE (slot_no = ? AND instructor = ? AND course = ? AND week = ? AND student = ?)", slot, ins, course, wid, stu);
			}
		}

		public void ClearTodayAttendance (enroll_view ev, int wid)
		{
			string ins = ev.instructor;
			int slot = ev.slot_no;
			string course = ev.course;
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended = 'Absent' WHERE (slot_no = ? AND instructor = ? AND course = ? AND week = ?)", slot, ins, course, wid);
			}
		}

		public void AddWeeklyAttendance (WeeklyAttendance w)
		{
			lock (locker) {
				_connection.Insert (w);
			}

		}

		//Other Methods
		//****************************************************************************************************

		public bool MemberExists (string email)
		{
			if (email.Contains ("@student.guc.edu.eg")) {
				IEnumerable<Student> sqlstudents = this.GetStudents ();
				foreach (var b in sqlstudents) {
					if (email.Equals (b.email)) {
						return true;
					}
				}
			} else {
				IEnumerable<Instructor> sqlinstructors = this.GetInstructors ();
				foreach (var b in sqlinstructors) {
					if (email.Equals (b.email)) {
						return true;
					}
				}
			}
			return false;
		}

		public string GetMemberPosition (string email)
		{
			if (email.Contains ("@student.guc.edu.eg")) {
				return "Student";
			} else {
				return "Instructor";
			}
		}

		public bool PasswordCorrect (string email, string password)
		{
			if (email.Contains ("@student.guc.edu.eg")) {
				IEnumerable<Student> sqlstudents = this.GetStudents ();
				foreach (var b in sqlstudents) {
					if (email.Equals (b.email)) {
						if (password.Equals (b.password)) {
							return true;
						}
					}
				}
			} else {
				IEnumerable<Instructor> sqlinstructors = this.GetInstructors ();
				foreach (var b in sqlinstructors) {
					if (email.Equals (b.email)) {
						if (password.Equals (b.password)) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool InstructorTeaches (int id)
		{
			IEnumerable<enroll> t = this.GetEnroll ();
			foreach (var b in t) {
				if (b.tid == id) {
					return true;
				}
			}
			return false;
		}

		public bool StudentTakes (string id)
		{
			IEnumerable<enroll> t = this.GetEnroll ();
			foreach (var b in t) {
				if (b.sid.Equals (id)) {
					return true;
				}
			}
			return false;
		}

		public bool StudentTakesToday (string name, string day)
		{
			IEnumerable<enroll_view> t = this.GetEnrollView ();
			foreach (var b in t) {
				if (b.student.Equals (name) && b.slot.Contains (day)) {
					return true;
				}
			}
			return false;
		}

		public bool InstructorTakesToday (string name, string day)
		{
			IEnumerable<enroll_view> t = this.GetEnrollView ();
			foreach (var b in t) {
				if (b.instructor.Equals (name) && b.slot.Contains (day)) {
					return true;
				}
			}
			return false;
		}


		public void updateSlotNumber ()
		{
			lock (locker) {
				foreach (var a in this.GetEnrollView ()) {
					int slot_no = this.GetSlotNumber (a.slot);
					_connection.Execute ("UPDATE enroll_view Set slot_no  = ? WHERE id = ?", slot_no, a.id);
				}
				foreach (var a in this.GetWeeklyAttendance ()) {
					int slot_no = this.GetSlotNumber (a.slot);
					_connection.Execute ("UPDATE WeeklyAttendance Set slot_no  = ? WHERE id = ?", slot_no, a.id);
				}
			}
		}

		public void UpdateWeeklyAttendance (string id, string attended)
		{
			lock (locker) {
				_connection.Execute ("UPDATE WeeklyAttendance Set attended  = ? WHERE id = ?", attended, id);
			}
			
		}

	}
}

