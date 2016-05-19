// Smart Tutorial Attendance System
// Created By: Zeyad Ahmed Atef
// Started: February 2016

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using GUC_Attendance.Models;

namespace GUC_Attendance
{
	public class GUC_Attendance_Manager
	{
		
		public MobileServiceClient client;
		public IMobileServiceTable<Student> studentsAPITable;
		public IMobileServiceTable<Instructor> instructorsAPITable;
		public IMobileServiceTable<Course> coursesAPITable;
		public IMobileServiceTable<Slot> slotsAPITable;
		public IMobileServiceTable<enroll> enrollAPITable;
		public IMobileServiceTable<enroll_view> enroll_viewAPITable;
		public IMobileServiceTable<WeeklyAttendance> weeklyattendanceAPITable;
		public IMobileServiceTable<Week> weeksAPITable;





		public GUC_Attendance_Manager ()
		{
			//API Initilization
			client = new MobileServiceClient (Constants.ApplicationURL, Constants.ApplicationKey);

			studentsAPITable = client.GetTable<Student> ();
			instructorsAPITable = client.GetTable<Instructor> ();
			coursesAPITable = client.GetTable<Course> ();
			slotsAPITable = client.GetTable<Slot> ();
			enrollAPITable = client.GetTable<enroll> ();
			enroll_viewAPITable = client.GetTable<enroll_view> ();
			weeklyattendanceAPITable = client.GetTable<WeeklyAttendance> ();
			weeksAPITable = client.GetTable<Week> ();

		}


		public async Task AddMember (Member m)
		{
			if (m.position.Equals ("Student")) {
				bool studentexists = await this.StudentExists (m.id);
				var s = new Student {
					sid = m.id,
					email = m.email,
					fname = m.fname,
					lname = m.lname,
					password = m.password
				};
				if (!studentexists) {
					await studentsAPITable.InsertAsync (s);
				} else {
				}

			} else {
				var t = new Instructor () {
					email = m.email,
					fname = m.fname,
					lname = m.lname,
					password = m.password
				};


				await instructorsAPITable.InsertAsync (t);
			}

		}

		//Methods on Table: Student
		//****************************************************************************************************

		public async Task<ObservableCollection<Student>> GetAllStudents (int i)
		{
			try {
				return new ObservableCollection<Student> (
					await studentsAPITable.Take (100).Where (a => a.count >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<String> GetStudentName (String id)
		{
			var querystudents = await studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<Student>> test = this.GetAllStudents (i);
				ObservableCollection<Student> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.sid.Equals (id)) {
						return xo.fname + " " + xo.lname;
					}
				}
			}
			return null;
		}

		public async Task<Student> GetStudentAsync (string id)
		{
			try {
				return await studentsAPITable.LookupAsync (id);
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<bool> StudentExists (string id)
		{
			var querystudents = await studentsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countstudents = ((ITotalCountProvider)querystudents).TotalCount;
			for (int i = 1; i <= countstudents; i += 100) {
				Task<ObservableCollection<Student>> test = this.GetAllStudents (i);
				ObservableCollection<Student> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.sid.Equals (id)) {
						return true;
					}
				}
			}
			return false;
		}

		//Methods on Table: Instructor
		//****************************************************************************************************

		public async Task<ObservableCollection<Instructor>> GetAllInstructors (int i)
		{
			try {
				return new ObservableCollection<Instructor> (
					await instructorsAPITable.Take (100).Where (a => a.tid >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<String> GetInstructorName (int id)
		{
			var queryinstructors = await instructorsAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countinstructors = ((ITotalCountProvider)queryinstructors).TotalCount;
			for (int i = 1; i <= countinstructors; i += 100) {
				Task<ObservableCollection<Instructor>> test = this.GetAllInstructors (i);
				ObservableCollection<Instructor> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.tid == id) {
						return xo.fname + " " + xo.lname;
					}
				}
			}
			return null;
		}

		//Methods on Table: Course
		//****************************************************************************************************

		public async Task<ObservableCollection<Course>> GetAllCourses (int i)
		{
			try {
				return new ObservableCollection<Course> (
					await coursesAPITable.Take (100).Where (a => a.cid >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<String> GetCourseNameAndCode (int id)
		{
			var querycourses = await coursesAPITable.Take (1).IncludeTotalCount ().ToEnumerableAsync (); 
			long countcourses = ((ITotalCountProvider)querycourses).TotalCount;
			for (int i = 1; i <= countcourses; i += 100) {
				Task<ObservableCollection<Course>> test = this.GetAllCourses (i);
				ObservableCollection<Course> zzz = await test;
				foreach (var xo in zzz) {
					if (xo.cid == id) {
						return xo.code + ": " + xo.name;
					}
				}
			}
			return null;
		}

		//Methods on Table: Slot
		//****************************************************************************************************

		public async Task<ObservableCollection<Slot>> GetAllSlots ()
		{
			try {
				return new ObservableCollection<Slot> (
					await slotsAPITable.ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<String> GetSlotName (int id)
		{
			Task<ObservableCollection<Slot>> test = this.GetAllSlots ();
			ObservableCollection<Slot> zzz = await test;
			foreach (var xo in zzz) {
				if (xo.slot_no == id) {
					return xo.day + " " + xo.timing;
				}
			}
			return null;
		}

		public async Task<ObservableCollection<Week>> GetAllWeeks ()
		{
			try {
				return new ObservableCollection<Week> (
					await weeksAPITable.ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task SetWeeks ()
		{
			Week a = new Week {
				week_no = 1,
				start = "30/01/2016"	
			};
			Week b = new Week {
				week_no = 2,
				start = "06/02/2016"	
			};
			Week c = new Week {
				week_no = 3,
				start = "13/02/2016"	
			};
			Week d = new Week {
				week_no = 4,
				start = "20/02/2016"	
			};
			Week e = new Week {
				week_no = 5,
				start = "27/02/2016"	
			};
			Week f = new Week {
				week_no = 6,
				start = "12/03/2016"	
			};
			Week g = new Week {
				week_no = 7,
				start = "19/03/2016"	
			};
			Week h = new Week {
				week_no = 8,
				start = "26/03/2016"	
			};
			Week i = new Week {
				week_no = 9,
				start = "02/04/2016"	
			};
			Week j = new Week {
				week_no = 10,
				start = "09/04/2016"	
			};
			Week k = new Week {
				week_no = 11,
				start = "16/04/2016"	
			};
			Week l = new Week {
				week_no = 12,
				start = "23/04/2016"	
			};

			await weeksAPITable.InsertAsync (a);
			await weeksAPITable.InsertAsync (b);
			await weeksAPITable.InsertAsync (c);
			await weeksAPITable.InsertAsync (d);
			await weeksAPITable.InsertAsync (e);
			await weeksAPITable.InsertAsync (f);
			await weeksAPITable.InsertAsync (g);
			await weeksAPITable.InsertAsync (h);
			await weeksAPITable.InsertAsync (i);
			await weeksAPITable.InsertAsync (j);
			await weeksAPITable.InsertAsync (k);
			await weeksAPITable.InsertAsync (l);


		}


		//		public async Task setslots ()
		//		{
		//			int check = 1;
		//			string d = "";
		//			string t = "";
		//			int n = 1;
		//
		//			for (int i = 0; i < 6; i++)
		//			{
		//				switch (check)
		//				{
		//				case 1:
		//					d = "Saturday";
		//					break;
		//				case 2:
		//					d = "Sunday";
		//					break;
		//				case 3:
		//					d = "Monday";
		//					break;
		//				case 4:
		//					d = "Tuesday";
		//					break;
		//				case 5:
		//					d = "Wednesday";
		//					break;
		//				case 6:
		//					d = "Thursday";
		//					break;
		//				}
		//
		//				for (int j = 0; j < 5; j++)
		//				{
		//					switch (j) {
		//					case 0:
		//						t = "1st";
		//						break;
		//					case 1:
		//						t = "2nd";
		//						break;
		//					case 2:
		//						t = "3rd";
		//						break;
		//					case 3:
		//						t = "4th";
		//						break;
		//					case 4:
		//						t = "5th";
		//						break;
		//					}
		//					var slot = new Slot {
		//						day = d,
		//						timing = t,
		//						slot_no = n
		//					};
		//					n++;
		//					await slotsAPITable.InsertAsync (slot);
		//				}
		//
		//				check++;
		//
		//			}
		//
		//		}


		//Methods on Table: enroll
		//****************************************************************************************************

		public async Task<ObservableCollection<enroll>> GetAllEnroll (int i)
		{
			try {
				return new ObservableCollection<enroll> (
					await enrollAPITable.Take (100).Where (a => a.eid >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<ObservableCollection<enroll>> GetAllEnrollDeleted (int i)
		{
			try {
				return new ObservableCollection<enroll> (
					await enrollAPITable.Take (100).Where (a => a.eid >= i).IncludeDeleted ().ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<ObservableCollection<enroll_view>> GetAllEnrollView (int i)
		{
			try {
				return new ObservableCollection<enroll_view> (
					await enroll_viewAPITable.Take (100).Where (a => a.eid >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task<ObservableCollection<WeeklyAttendance>> GetAllWeeklyAttendance (int i)
		{
			try {
				return new ObservableCollection<WeeklyAttendance> (
					await weeklyattendanceAPITable.Take (100).Where (a => a.wid >= i).ToListAsync ());
			} catch (MobileServiceInvalidOperationException msioe) {
				Debug.WriteLine (@"INVALID {0}", msioe.Message);
			} catch (Exception e) {
				Debug.WriteLine (@"ERROR {0}", e.Message);
			}
			return null;
		}

		public async Task DeleteEnroll (int eid)
		{
			List<enroll> enrolllist = await enrollAPITable.Where (a => a.eid == eid).ToListAsync ();
			foreach (enroll item in enrolllist) {
				await enrollAPITable.DeleteAsync (item);
			}
			return;
		}

		public async Task DeleteEnrollView (int eid)
		{
			List<enroll_view> enrollviewlist = await enroll_viewAPITable.Where (a => a.eid == eid).ToListAsync ();
			foreach (enroll_view item in enrollviewlist) {
				await enroll_viewAPITable.DeleteAsync (item);
			}
			return;
		}

		public async Task DeleteWeeklyAttendance (int eid)
		{
			List<WeeklyAttendance> weeklyattendancelist = await weeklyattendanceAPITable.Take (500).Where (a => a.eid == eid).ToListAsync ();
			foreach (WeeklyAttendance item in weeklyattendancelist) {
				await weeklyattendanceAPITable.DeleteAsync (item);
			}
			return;
		}


		public async Task<List<enroll>> GetEnrollList (int tid, int cid, int slot_no, string tutorial)
		{
			return await enrollAPITable.Where (a => a.tid == tid && a.cid == cid && a.slot_no == slot_no && a.tutorial == tutorial).ToListAsync ();

		}

		public async Task<enroll> GetEnroll (int eid)
		{
			List<enroll> list = await enrollAPITable.Where (a => a.eid == eid).ToListAsync ();
			return list [0];
		}

		public async Task undelete ()
		{
			List<enroll> l = await enrollAPITable.IncludeDeleted ().Take (100).Where (a => a.tid == 2).ToListAsync ();
			foreach (enroll item in l) {
				await enrollAPITable.UndeleteAsync (item);
			}

			List<enroll_view> ll = await enroll_viewAPITable.IncludeDeleted ().Where (a => a.instructor == "Rana Helal").ToListAsync ();
			foreach (enroll_view item in ll) {
				await enroll_viewAPITable.UndeleteAsync (item);
			}

			List<WeeklyAttendance> lw = await weeklyattendanceAPITable.IncludeDeleted ().Take (1000).Where (a => a.instructor == "Rana Helal").ToListAsync ();
			foreach (WeeklyAttendance item in lw) {
				await weeklyattendanceAPITable.UndeleteAsync (item);
			}
			return;
		}

	}
}
	