using System;
using System.Collections.Generic;
using System.IO;

namespace NoteList.DAL
{
	public class NoteItemRepository
	{
		NoteDatabase db = null;
		protected static string dbLocation;		
		protected static NoteItemRepository me;		

		static NoteItemRepository ()
		{
			me = new NoteItemRepository();
		}

		protected NoteItemRepository ()
		{
			// set the db location
			dbLocation = DatabaseFilePath;

			// instantiate the database	
			db = new NoteDatabase(dbLocation);
		}

		public static string DatabaseFilePath 
		{
			get 
			{ 
				var sqliteFilename = "NoteDatabase1.db3";

				#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
				#else

				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif

				#endif

				return path;	
			}
		}

		public static NoteItem GetNote(int id)
		{
			return me.db.GetItem(id);
		}

		public static IEnumerable<NoteItem> GetNotes()
		{
			return me.db.GetItems();
		}

		public static int SaveNote(NoteItem item)
		{
			return me.db.SaveItem(item);
		}

		public static int DeleteNote(int id)
		{
			return me.db.DeleteItem(id);
		}
	}
}

