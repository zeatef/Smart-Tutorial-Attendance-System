// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Net;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using GUC_Attendance.Models;
using Acr.UserDialogs;
using Splat;


namespace GUC_Attendance
{
	public class SQL_API_Manager
	{
		public GUC_Attendance_Manager apimanager = new GUC_Attendance_Manager ();
		SQLDatabase sqldatabase;




		public SQL_API_Manager (SQLDatabase sqldatabase)
		{
			this.sqldatabase = sqldatabase;
		}

		public async Task fetchDataFromAPItoSQL ()
		{
			Debug.WriteLine ("Started Fetching...");
			var querystudents = await apimanager.studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			List<Student> allstudentsinapi = new List<Student> ();
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<Student>> apistudents = apimanager.GetAllStudents (i);
				ObservableCollection<Student> ostudents = await apistudents;
				IEnumerable<Student> sqlstudents = sqldatabase.GetStudents ();
				foreach (var a in ostudents) {
					allstudentsinapi.Add (a);
					bool studentexists = false;
					foreach (var b in sqlstudents) {
						if (a.id.Equals (b.id)) {
							studentexists = true;
							if (a.email != null) {
								if (b.email == null) {
									sqldatabase.UpdateStudentEmail (b.sid, a.email);
									sqldatabase.UpdateStudentPassword (b.sid, a.password);
								}
							}
							break;
						}
					}
					if (!studentexists) {
						var newStudent = new Student {
							count = a.count,
							id = a.id,
							sid = a.sid,
							fname = a.fname,
							lname = a.lname,
							email = a.email,
							password = a.password
						};
						sqldatabase.AddStudent (newStudent);
					}
				}
			}
			IEnumerable<Student> allsqlstudents = sqldatabase.GetStudents ();
			foreach (var a in allsqlstudents) {
				bool studentexistsinapi = false;
				foreach (var b in allstudentsinapi) {
					if (a.id.Equals (b.id)) {
						studentexistsinapi = true;
						break;
					}
				}
				if (!studentexistsinapi) {
					sqldatabase.DeleteStudent (a.id);
				}
			}
			var queryinstructors = await apimanager.instructorsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countinstructors = ((ITotalCountProvider)queryinstructors).TotalCount;
			List<Instructor> allinstructorinapi = new List<Instructor> ();
			for (int i = 1; i <= countinstructors; i += 100) {
				Task<ObservableCollection<Instructor>> apiinstructors = apimanager.GetAllInstructors (i);
				ObservableCollection<Instructor> oinstructors = await apiinstructors;
				IEnumerable<Instructor> sqlinstructors = sqldatabase.GetInstructors ();
				foreach (var a in oinstructors) {
					allinstructorinapi.Add (a);
					bool instructorexists = false;
					foreach (var b in sqlinstructors) {
						if (a.id.Equals (b.id)) {
							instructorexists = true;
							break;
						}
					}
					if (!instructorexists) {
						var newInstructor = new Instructor () {
							id = a.id,
							fname = a.fname,
							lname = a.lname,
							email = a.email,
							password = a.password
						};
						sqldatabase.AddInstructor (newInstructor);
					}
				}
			}
			IEnumerable<Instructor> allsqlinstructors = sqldatabase.GetInstructors ();
			foreach (var a in allsqlinstructors) {
				bool instructorexistsinapi = false;
				foreach (var b in allinstructorinapi) {
					if (a.id.Equals (b.id)) {
						instructorexistsinapi = true;
						break;
					}
				}
				if (!instructorexistsinapi) {
					sqldatabase.DeleteInstructor (a.id);
				}
			}
			var querycourses = await apimanager.coursesAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countcourses = ((ITotalCountProvider)querycourses).TotalCount;
			List<Course> allcoursesinapi = new List<Course> ();
			for (int i = 1; i <= countcourses; i += 100) {
				Task<ObservableCollection<Course>> apicourses = apimanager.GetAllCourses (i);
				ObservableCollection<Course> ocourses = await apicourses;
				IEnumerable<Course> sqlcourses = sqldatabase.GetCourses ();
				foreach (var a in ocourses) {
					allcoursesinapi.Add (a);
					bool courseexists = false;
					foreach (var b in sqlcourses) {
						if (a.id.Equals (b.id)) {
							courseexists = true;
							break;
						}
					}
					if (!courseexists) {
						var newCourse = new Course () {
							id = a.id,
							cid = a.cid,
							code = a.code,
							name = a.name
						};
						sqldatabase.AddCourse (newCourse);
					}
				}
			}
			IEnumerable<Course> allsqlcourses = sqldatabase.GetCourses ();
			foreach (var a in allsqlcourses) {
				bool courseexistsinapi = false;
				foreach (var b in allcoursesinapi) {
					if (a.id.Equals (b.id)) {
						courseexistsinapi = true;
						break;
					}
				}
				if (!courseexistsinapi) {
					sqldatabase.DeleteCourse (a.id);
				}
			}
			Task<ObservableCollection<Slot>> apislots = apimanager.GetAllSlots ();
			ObservableCollection<Slot> oslots = await apislots;
			IEnumerable<Slot> sqlslots = sqldatabase.GetSlots ();
			foreach (var a in oslots) {
				bool slotexists = false;
				foreach (var b in sqlslots) {
					if (a.id.Equals (b.id)) {
						slotexists = true;
						break;
					}
				}
				if (!slotexists) {
					var newSlot = new Slot () {
						id = a.id,
						day = a.day,
						timing = a.timing,
						slot_no = a.slot_no
					};
					sqldatabase.AddSlot (newSlot);
				}
			}
			Task<ObservableCollection<Week>> apiweeks = apimanager.GetAllWeeks ();
			ObservableCollection<Week> oweeks = await apiweeks;
			IEnumerable<Week> sqlweeks = sqldatabase.GetWeeks ();
			foreach (var a in oweeks) {
				bool weekexists = false;
				foreach (var b in sqlweeks) {
					if (a.id.Equals (b.id)) {
						weekexists = true;
						break;
					}
				}
				if (!weekexists) {
					var newWeek = new Week () {
						id = a.id,
						start = a.start,
						week_no = a.week_no
					};
					sqldatabase.AddWeek (newWeek);
				}
			}
			var queryenroll = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countenroll = ((ITotalCountProvider)queryenroll).TotalCount;
			List<enroll> allenrollinapi = new List<enroll> ();
			for (int i = 1; i <= countenroll; i += 100) {
				Task<ObservableCollection<enroll>> apienroll = apimanager.GetAllEnroll (i);
				ObservableCollection<enroll> oenroll = await apienroll;
				IEnumerable<enroll> sqlenroll = sqldatabase.GetEnroll ();
				foreach (var a in oenroll) {
					allenrollinapi.Add (a);
					bool enrollexists = false;
					foreach (var b in sqlenroll) {
						if (a.id.Equals (b.id)) {
							enrollexists = true;
							break;
						}
					}
					if (!enrollexists) {
						var newEnroll = new enroll () {
							id = a.id,
							eid = a.eid,
							room = a.room,
							sid = a.sid,
							tid = a.tid,
							cid = a.cid,
							slot_no = a.slot_no,
							tutorial = a.tutorial
						};
						sqldatabase.AddEnroll (newEnroll);
					}
				}
			}
			IEnumerable<enroll> allsqlenroll = sqldatabase.GetEnroll ();
			foreach (var a in allsqlenroll) {
				bool enrollexistsinapi = false;
				foreach (var b in allenrollinapi) {
					if (a.id.Equals (b.id)) {
						enrollexistsinapi = true;
						break;
					}
				}
				if (!enrollexistsinapi) {
					sqldatabase.DeleteEnroll (a.id);
				}
			}
			var queryenrollview = await apimanager.enroll_viewAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countenrollview = ((ITotalCountProvider)queryenrollview).TotalCount;
			List<enroll_view> allenrollviewinapi = new List<enroll_view> ();
			for (int i = 1; i <= countenrollview; i += 100) {
				Task<ObservableCollection<enroll_view>> apienrollview = apimanager.GetAllEnrollView (i);
				ObservableCollection<enroll_view> oenrollview = await apienrollview;
				IEnumerable<enroll_view> sqlenrollview = sqldatabase.GetEnrollView ();
				foreach (var a in oenrollview) {
					allenrollviewinapi.Add (a);
					bool enrollviewexists = false;
					foreach (var b in sqlenrollview) {
						if (a.id.Equals (b.id)) {
							enrollviewexists = true;
							break;
						}
					}
					if (!enrollviewexists) {
						var tv = new enroll_view {
							id = a.id,
							eid = a.eid,
							student = a.student,
							course = a.course,
							instructor = a.instructor,
							room = a.room,
							slot = a.slot,
							slot_no = a.slot_no,
							tutorial = a.tutorial
						};

						sqldatabase.AddEnrollView (tv);
					}
				}
			}
			IEnumerable<enroll_view> allsqlenrollview = sqldatabase.GetEnrollView ();
			foreach (var a in allsqlenrollview) {
				bool enrollviewexistsinapi = false;
				foreach (var b in allenrollviewinapi) {
					if (a.id.Equals (b.id)) {
						enrollviewexistsinapi = true;
						break;
					}
				}
				if (!enrollviewexistsinapi) {
					sqldatabase.DeleteEnrollView (a.id);
				}
			}
			var queryweeklyattendance = await apimanager.weeklyattendanceAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			List<WeeklyAttendance> allweeklyattendanceinapi = new List<WeeklyAttendance> ();
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<WeeklyAttendance>> apiweeklyattendance = apimanager.GetAllWeeklyAttendance (i);
				ObservableCollection<WeeklyAttendance> oweeklyattendance = await apiweeklyattendance;
				IEnumerable<WeeklyAttendance> sqlweeklyattendance = sqldatabase.GetWeeklyAttendance ();
				foreach (var a in oweeklyattendance) {
					allweeklyattendanceinapi.Add (a);
					bool weeklyattendanceexists = false;
					foreach (var b in sqlweeklyattendance) {
						if (a.id.Equals (b.id)) {
							if (!a.attended.Equals (b.attended)) {
								sqldatabase.UpdateWeeklyAttendance (a.id, a.attended);
							}
							weeklyattendanceexists = true;
							break;
						}
					}
					if (!weeklyattendanceexists) {
						var w = new WeeklyAttendance {
							id = a.id,
							wid = a.wid,
							eid = a.eid,
							week = a.week,
							student = a.student,
							course = a.course,
							instructor = a.instructor,
							room = a.room,
							slot = a.slot,
							attended = a.attended,
							slot_no = a.slot_no,
							tutorial = a.tutorial
						};

						sqldatabase.AddWeeklyAttendance (w);
					} else {
						
					}
				}
			}

			IEnumerable<WeeklyAttendance> allsqlweeklyattendance = sqldatabase.GetWeeklyAttendance ();
			foreach (var a in allsqlweeklyattendance) {
				bool weeklyattendanceexistsinapi = false;
				foreach (var b in allweeklyattendanceinapi) {
					if (a.id.Equals (b.id)) {
						weeklyattendanceexistsinapi = true;
						break;
					}
				}
				if (!weeklyattendanceexistsinapi) {
					sqldatabase.DeleteWeeklyAttendance (a.id);
				}
			}
			Debug.WriteLine ("Finished Fetching.");
		}


		public async Task AddMember (Member m)
		{
			bool exists = false;
			if (m.position.Equals ("Student")) {
				var querystudents = await apimanager.studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
				long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
				for (int i = 1; i <= countstudents; i += 100) {
					Task<ObservableCollection<Student>> apistudents = apimanager.GetAllStudents (i);
					ObservableCollection<Student> ostudents = await apistudents;
					foreach (var b in ostudents) {
						if (m.id.Equals (b.sid)) {
							exists = true;
							break;
						}
					}
					if (!exists) {
						var newStudent = new Student {
							count = (int)countstudents++,
							sid = m.id,
							fname = m.fname,
							lname = m.lname,
							email = m.email,
							password = m.password
						};
						await apimanager.studentsAPITable.InsertAsync (newStudent);
					}
				}
			} else {
				var t = new Instructor () {
					email = m.email,
					fname = m.fname,
					lname = m.lname,
					password = m.password
				};
				sqldatabase.AddInstructor (t);
				t.tid = sqldatabase.GetInstructorID (t.email);
				await apimanager.instructorsAPITable.InsertAsync (t);
				var queryinstructors = await apimanager.instructorsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
				long countinstructors = ((ITotalCountProvider)queryinstructors).TotalCount;
				for (int i = 1; i <= countinstructors; i += 100) {
					Task<ObservableCollection<Instructor>> apiinstructors = apimanager.GetAllInstructors (i);
					ObservableCollection<Instructor> oinstructors = await apiinstructors;
					foreach (var b in oinstructors) {
						if (t.tid == b.tid) {
							sqldatabase.UpdateInstructorID (t.tid, b.id);
							break;
						}
					}
				}
			}
		}

		//Methods on Api Table: Student
		//***************************************************************************

		public async Task AddStudent (Student s)
		{
			await apimanager.studentsAPITable.InsertAsync (s);
		}

		public async Task<String> GetStudentName (String id)
		{
			var querystudents = await apimanager.studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<Student>> test = apimanager.GetAllStudents (i);
				ObservableCollection<Student> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.sid.Equals (id)) {
						return xo.fname + " " + xo.lname;
					}
				}
			}
			return null;
		}

		public async Task<bool> StudentExists (string sid)
		{
			var querystudents = await apimanager.studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<Student>> test = apimanager.GetAllStudents (i);
				ObservableCollection<Student> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.sid.Equals (sid)) {
						return true;
					}
				}
			}
			return false;
		}

		public bool StudentAlreadySignedUp (string sid)
		{
			IEnumerable<Student> sqlstudents = sqldatabase.GetStudents ();
			foreach (var b in sqlstudents) {
				if (b.sid.Equals (sid)) {
					if (b.email != null) {
						return true;
					} else {
						return false;
					}
				}
			}
			return false;
		}

		public async Task UpdateExistingStudent (Member s)
		{
			Debug.WriteLine ("Check");
			ObservableCollection<Student> checkk = new ObservableCollection<Student> (await apimanager.studentsAPITable.Where (a => a.sid == s.id).Take (1).ToListAsync ());
			checkk [0].email = s.email;
			checkk [0].password = s.password;
			await apimanager.studentsAPITable.UpdateAsync (checkk [0]);
			IEnumerable<Student> sqlstudents = sqldatabase.GetStudents ();
//			foreach (Student a in sqlstudents) {
//				if(a.sid.Equals (s.id)) {
//					a.email = s.email;
//					a.password = s.password;
//					break;
//				}
//			}
			Debug.WriteLine ("Check22");
			return;
		}

		public async Task Zodiac ()
		{
			await this.UpdateExistingStudent (new Member {
				email = "a.i@student.guc.edu.eg",
				password = "12345678",
				id = "28-11797"
			});
			await this.UpdateExistingStudent (new Member {
				email = "f.m@student.guc.edu.eg",
				password = "12345678",
				id = "28-4350"
			});
			await this.UpdateExistingStudent (new Member {
				email = "h.a@student.guc.edu.eg",
				password = "12345678",
				id = "28-4471"
			});
			await this.UpdateExistingStudent (new Member {
				email = "h.b@student.guc.edu.eg",
				password = "12345678",
				id = "28-0741"
			});
			await this.UpdateExistingStudent (new Member {
				email = "hesham.a@student.guc.edu.eg",
				password = "12345678",
				id = "22-0997"
			});
			await this.UpdateExistingStudent (new Member {
				email = "l.z@student.guc.edu.eg",
				password = "12345678",
				id = "28-1256"
			});

			return;

		}

		public async Task<int> GetStudentsNumberOfRows ()
		{
			var query = await apimanager.studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			int count = ((int)((ITotalCountProvider)query).TotalCount);
			return count;
		}


		//Methods on Api Table: Course
		//***************************************************************************

		public async Task AddCourse (Course c)
		{
			sqldatabase.AddCourse (c);
			c.cid = sqldatabase.GetCourseID (c.code);
			await apimanager.coursesAPITable.InsertAsync (c);
			var querycourses = await apimanager.coursesAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countcourses = ((ITotalCountProvider)querycourses).TotalCount;
			for (int i = 1; i <= countcourses; i += 100) {
				Task<ObservableCollection<Course>> apicourses = apimanager.GetAllCourses (i);
				ObservableCollection<Course> ocourses = await apicourses;
				foreach (var b in ocourses) {
					if (c.cid == b.cid) {
						sqldatabase.UpdateCourseID (c.cid, b.id);
						return;
					}
				}
			}
		}

		public async Task<String> GetCourseNameAndCode (int id)
		{
			var querycourses = await apimanager.coursesAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countcourses = ((ITotalCountProvider)querycourses).TotalCount;
			for (int i = 1; i <= countcourses; i += 100) {
				Task<ObservableCollection<Course>> test = apimanager.GetAllCourses (i);
				ObservableCollection<Course> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.cid == id) {
						return xo.code + ": " + xo.name;
					}
				}
			}
			return null;
		}



		public bool CourseExists (string code)
		{
			IEnumerable<Course> sqlcourses = sqldatabase.GetCourses ();
			foreach (var b in sqlcourses) {
				if (b.code.Equals (code)) {
					return true;
				}
			}
			return false;
		}

		//Methods on Api Table: Instructor
		//***************************************************************************

		public async Task<String> GetInstructorName (int id)
		{
			var queryinstructors = await apimanager.instructorsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countinstructors = ((ITotalCountProvider)queryinstructors).TotalCount;
			for (int i = 1; i <= countinstructors; i += 100) {
				Task<ObservableCollection<Instructor>> test = apimanager.GetAllInstructors (i);
				ObservableCollection<Instructor> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.tid == id) {
						return xo.fname + " " + xo.lname;
					}
				}
			}
			return null;
		}

		public bool InstructorExists (string email)
		{
			IEnumerable<Instructor> sqlinstructors = sqldatabase.GetInstructors ();
			foreach (var b in sqlinstructors) {
				if (b.email.Equals (email)) {
					return true;
				}
			}
			return false;
		}

		//Methods on Api Table: Slot
		//***************************************************************************

		public async Task<String> GetSlotName (int id)
		{
			Task<ObservableCollection<Slot>> test = apimanager.GetAllSlots ();
			ObservableCollection<Slot> zzz = await test;
			foreach (var xo in zzz) {
				if (xo.slot_no == id) {
					return xo.day + " " + xo.timing;
				}
			}
			return null;
		}

		public int stringToSlotNumber (string day, string slot)
		{
			IEnumerable<Slot> sqlslot = sqldatabase.GetSlots ();
			foreach (var b in sqlslot) {
				if (b.day.Equals (day) && b.timing.Equals (slot)) {
					return b.slot_no;
				}
			}
			return 0;
		}

		//Methods on Api Table: enroll
		//***************************************************************************

		public async Task AddEnroll (enroll e)
		{
			await apimanager.enrollAPITable.InsertAsync (e);
		}

		public async Task InsertEnrollTableToAPI ()
		{
			IEnumerable<enroll> sqlenroll = sqldatabase.GetEnroll ();
			foreach (enroll a in sqlenroll) {
				await apimanager.enrollAPITable.InsertAsync (a);
			}

		}

		public async void UpdateEnrollTableID ()
		{
			var queryenroll = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countenroll = ((ITotalCountProvider)queryenroll).TotalCount;
			for (int i = 1; i <= countenroll; i += 100) {
				Task<ObservableCollection<enroll>> apienroll = apimanager.GetAllEnroll (i);
				ObservableCollection<enroll> oenroll = await apienroll;
				IEnumerable<enroll> sqlenroll = sqldatabase.GetEnroll ();
				foreach (var b in sqlenroll) {
					foreach (var a in oenroll) {
						if (a.eid == b.eid) {
							sqldatabase.UpdateEnrollID (b.eid, a.id);
							break;
						}
					}
				}
			}
		}

		public async Task<int> GetEnrollNumberOfRows ()
		{
			var query = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			int count = ((int)((ITotalCountProvider)query).TotalCount);
			return count;
		}

		public async Task<bool> EnrollExists (int cid, string sid, int tid, int slot)
		{
			var querystudents = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<enroll>> test = apimanager.GetAllEnroll (i);
				ObservableCollection<enroll> zzz = await test;
				foreach (var b in zzz) {
					if (b.sid.Equals (sid) && b.cid == cid && b.tid == tid && b.slot_no == slot) {
						return true;
					}
				}
			}
			return false;
		}


		//Methods on Api Table: enroll_view
		//***************************************************************************


		public async Task AddEnrollView (enroll_view e)
		{
			await apimanager.enroll_viewAPITable.InsertAsync (e);
		}


		public async Task UpdateEnrollViewTable ()
		{
			IEnumerable<enroll> sqlenroll = sqldatabase.GetEnroll ();
			foreach (var b in sqlenroll) {
				bool exists = false;
				foreach (var a in sqldatabase.GetEnrollView ()) {
					if (a.eid == b.eid) {
						exists = true;
						break;
					}
				}
				if (!exists) {
					var tv = new enroll_view {
						eid = b.eid,
						student = await apimanager.GetStudentName (b.sid),
						course = await apimanager.GetCourseNameAndCode (b.cid),
						instructor = await apimanager.GetInstructorName (b.tid),
						room = b.room,
						slot = await apimanager.GetSlotName (b.slot_no)
					};

					await apimanager.enroll_viewAPITable.InsertAsync (tv);
				}
			}
		}

		public async Task<int> GetWeeklyAttendanceNumberOfRows ()
		{
			var query = await apimanager.weeklyattendanceAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			int count = ((int)((ITotalCountProvider)query).TotalCount);
			return count;
		}

		public async Task AddWeeklyAttendance (WeeklyAttendance w)
		{
			await apimanager.weeklyattendanceAPITable.InsertAsync (w);
		}

		public async Task UpdateTutorialGroup ()
		{
			Debug.WriteLine ("Start");
			var queryweeklyattendance = await apimanager.weeklyattendanceAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<WeeklyAttendance>> apiweeklyattendance = apimanager.GetAllWeeklyAttendance (i);
				ObservableCollection<WeeklyAttendance> oweeklyattendance = await apiweeklyattendance;
				foreach (WeeklyAttendance a in oweeklyattendance) {
					a.tutorial = "T-10";
					await apimanager.weeklyattendanceAPITable.UpdateAsync (a);

				}
			}
			Debug.WriteLine ("End");
			Debug.WriteLine ("Start");
			var queryenroll = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countenroll = ((ITotalCountProvider)queryenroll).TotalCount;
			for (int i = 1; i <= countenroll; i += 100) {
				Task<ObservableCollection<enroll>> apiweeklyattendance = apimanager.GetAllEnroll (i);
				ObservableCollection<enroll> oweeklyattendance = await apiweeklyattendance;
				foreach (enroll a in oweeklyattendance) {
					a.tutorial = "T-10";
					await apimanager.enrollAPITable.UpdateAsync (a);
				}
			}
			Debug.WriteLine ("End");
			Debug.WriteLine ("Start");
			var queryenrollview = await apimanager.enroll_viewAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countenrollview = ((ITotalCountProvider)queryenrollview).TotalCount;
			for (int i = 1; i <= countenrollview; i += 100) {
				Task<ObservableCollection<enroll_view>> apiweeklyattendance = apimanager.GetAllEnrollView (i);
				ObservableCollection<enroll_view> oweeklyattendance = await apiweeklyattendance;
				foreach (enroll_view a in oweeklyattendance) {
					a.tutorial = "T-10";
					await apimanager.enroll_viewAPITable.UpdateAsync (a);
				}
			}
			Debug.WriteLine ("End");
		}

		public async void UpdateSlotNumber ()
		{
//			var queryenrollview = await apimanager.enroll_viewAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
//			long countenrollview = ((ITotalCountProvider)queryenrollview).TotalCount;
//			for (int i = 1; i <= countenrollview; i += 100) {
//				Task<ObservableCollection<enroll_view>> apienrollview = apimanager.GetAllEnrollView (i);
//				ObservableCollection<enroll_view> oenrollview = await apienrollview;
//				foreach (enroll_view a in oenrollview) {
//					a.slot_no = sqldatabase.GetSlotNumber (a.slot);
//					await apimanager.enroll_viewAPITable.UpdateAsync (a);
//
//				}
//			}
			Debug.WriteLine ("Start");
			var queryweeklyattendance = await apimanager.weeklyattendanceAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<WeeklyAttendance>> apiweeklyattendance = apimanager.GetAllWeeklyAttendance (i);
				ObservableCollection<WeeklyAttendance> oweeklyattendance = await apiweeklyattendance;
				foreach (WeeklyAttendance a in oweeklyattendance) {
					if (a.slot_no <= 0) {
						a.slot_no = sqldatabase.GetSlotNumber (a.slot);
						await apimanager.weeklyattendanceAPITable.UpdateAsync (a);
					}

				}
			}
			Debug.WriteLine ("End");

		}

		public async Task DeleteClass (List<int> eids)
		{
			foreach (int i in eids) {
				await apimanager.DeleteEnroll (i);
				await apimanager.DeleteEnrollView (i);
				await apimanager.DeleteWeeklyAttendance (i);
			}
			return;
		}

		public async Task UpdateTodayAttendance (enroll_view e, int w)
		{
			try {
				IEnumerable<WeeklyAttendance> sqlweeklyattendance = sqldatabase.GetTodayAttendance (e, w);
				foreach (WeeklyAttendance item in sqlweeklyattendance) {
					await apimanager.weeklyattendanceAPITable.UpdateAsync (item);
				}
			} catch (Exception exc) {
				UserDialogs.Instance.Alert ("Slow Internet Connection - Please Try Again");
			}

		}

		public async Task<List<int>> Geteids (int tid, int cid, int slot_no, string tutorial)
		{
			List<enroll> e = await apimanager.GetEnrollList (tid, cid, slot_no, tutorial);
			List<int> eids = new List<int> ();
			foreach (enroll i in e) {
				eids.Add (i.eid);
			}
			return eids;

		}

		public async Task RemoveStudentFromClass (int eid)
		{
			await apimanager.DeleteEnroll (eid);
			await apimanager.DeleteEnrollView (eid);
			await apimanager.DeleteWeeklyAttendance (eid);
			return;
		}

		public async Task UpdateSlotLamia ()
		{
			var queryweeklyattendance = await apimanager.weeklyattendanceAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<WeeklyAttendance>> apiweeklyattendance = apimanager.GetAllWeeklyAttendance (i);
				ObservableCollection<WeeklyAttendance> oweeklyattendance = await apiweeklyattendance;
				foreach (WeeklyAttendance a in oweeklyattendance) {
					a.room = "C2.302";
					await apimanager.weeklyattendanceAPITable.UpdateAsync (a);

				}
			}
			return;
		}

		public async Task UpdateSlotLamia2 ()
		{
			var queryweeklyattendance = await apimanager.enroll_viewAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<enroll_view>> apiweeklyattendance = apimanager.GetAllEnrollView (i);
				ObservableCollection<enroll_view> oweeklyattendance = await apiweeklyattendance;
				foreach (enroll_view a in oweeklyattendance) {
					a.room = "C2.302";
					await apimanager.enroll_viewAPITable.UpdateAsync (a);
				}
			}
			return;
		}

		public async Task UpdateSlotLamia3 ()
		{
			var queryweeklyattendance = await apimanager.enrollAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countweeklyattendance = ((ITotalCountProvider)queryweeklyattendance).TotalCount;
			for (int i = 1; i <= countweeklyattendance; i += 100) {
				Task<ObservableCollection<enroll>> apiweeklyattendance = apimanager.GetAllEnroll (i);
				ObservableCollection<enroll> oweeklyattendance = await apiweeklyattendance;
				foreach (enroll a in oweeklyattendance) {
					a.room = "C2.302";
					await apimanager.enrollAPITable.UpdateAsync (a);
				}
			}
			return;
		}


		public async Task UpdateWeeks ()
		{
			DateTime now = DateTime.Now;
			now = now.AddDays (-3);
			Task<ObservableCollection<Week>> apiweeklyattendance = apimanager.GetAllWeeks ();
			ObservableCollection<Week> oweeklyattendance = await apiweeklyattendance;
			foreach (Week a in oweeklyattendance) {
				DateTime check = now.AddDays ((a.week_no - 1) * 7);
				string day;
				string month;
				if (check.Day < 10) {
					day = "0" + check.Day;
				} else {
					day = check.Day.ToString ();
				}
				if (check.Month < 10) {
					month = "0" + check.Month;
				} else {
					month = check.Month.ToString ();
				}
				a.start = day + "/" + month + "/" + check.Year;
				await apimanager.weeksAPITable.UpdateAsync (a);
			}

		}
			



	}
}

