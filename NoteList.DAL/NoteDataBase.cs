using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace NoteList.DAL
{
	/// <summary>
	/// TaskDatabase uses ADO.NET to create the [Items] table and create,read,update,delete data
	/// </summary>
	public class NoteDatabase 
	{
		static object locker = new object ();

		public SqliteConnection connection;

		public string path;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		public NoteDatabase (string dbPath) 
		{
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Description NTEXT, ImageUri NTEXT, Priority INTEGER, ToDoDate DATETIME, CreationDate DATETIME);"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						c.ExecuteNonQuery ();
					}
				}
			} else {
				// already exists, do nothing. 
			}
			Console.WriteLine (output);
		}

		/// <summary>Convert from DataReader to Task object</summary>
		NoteItem FromReader (SqliteDataReader r) {
			var t = new NoteItem ();
			t.ID = Convert.ToInt32 (r ["_id"]);
			t.Name = r ["Name"].ToString ();
			t.Description = r ["Description"].ToString ();
			t.ImageUri = r ["ImageUri"].ToString ();
			t.Priority = Convert.ToInt32(r ["Priority"]);
			t.ToDoDate = (DateTime)(r ["ToDoDate"]);
			t.CreationDate = (DateTime)(r ["CreationDate"]);
			return t;
		}

		public IEnumerable<NoteItem> GetItems ()
		{
			var tl = new List<NoteItem> ();

			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Description], [ImageUri], [Priority], [ToDoDate], [CreationDate] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						tl.Add (FromReader(r));
					}
				}
				connection.Close ();
			}
			return tl;
		}

		public NoteItem GetItem (int id) 
		{
			var t = new NoteItem ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "SELECT [_id], [Name], [Description], [ImageUri], [Priority], [ToDoDate], [CreationDate] from [Items] WHERE [_id] = ?";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					var r = command.ExecuteReader ();
					while (r.Read ()) {
						t = FromReader (r);
						break;
					}
				}
				connection.Close ();
			}
			return t;
		}

		public int SaveItem (NoteItem item) 
		{
			int r;
			lock (locker) {
				if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "UPDATE [Items] SET [Name] = ?, [Description] = ?, [ImageUri] = ?, [Priority] = ?, [ToDoDate] = ?, [CreationDate] = ? WHERE [_id] = ?;";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Description });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.ImageUri });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.Priority });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.ToDoDate });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.CreationDate });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				} else {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "INSERT INTO [Items] ([Name], [Description], [ImageUri], [Priority], [ToDoDate], [CreationDate]) VALUES (? ,?, ?, ?, ?, ?)";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Description });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.ImageUri });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.Priority });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.ToDoDate });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.CreationDate });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}

			}
		}

		public int DeleteItem(int id) 
		{
			lock (locker) {
				int r;
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "DELETE FROM [Items] WHERE [_id] = ?;";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id});
					r = command.ExecuteNonQuery ();
				}
				connection.Close ();
				return r;
			}
		}
	}
}