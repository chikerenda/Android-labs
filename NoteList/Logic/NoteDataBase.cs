using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace NoteList
{
	/// <summary>
	/// TaskDatabase uses ADO.NET to create the [Items] table and create,read,update,delete data
	/// </summary>
	public class NoteDatabase 
	{
		static object locker = new object ();

		public string path;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		public NoteDatabase (string dbPath) 
		{
		}

		/// <summary>Convert from DataReader to Task object</summary>
		NoteItem FromReader () {
			return new NoteItem ();
		}

		public IEnumerable<NoteItem> GetItems ()
		{
			return new List<NoteItem> ();
		}

		public NoteItem GetItem (int id) 
		{
			return new NoteItem ();
		}

		public int SaveItem (NoteItem item) 
		{
			return 0;
		}

		public int DeleteItem(int id) 
		{
			return 0;
		}
	}
}

