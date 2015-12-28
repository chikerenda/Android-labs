using System;

namespace NoteList.DAL
{
	/// <summary>
	/// Note data model
	/// </summary>
	/// <value>The priority: 0 - Minor, 1 - Major, 2 - Critical</value>
	public class NoteItem
	{
		public NoteItem ()
		{
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageUri { get; set; }
		public int Priority { get; set; }
		public DateTime ToDoDate { get; set; }
		public DateTime CreationDate { get; set; }
	}
}