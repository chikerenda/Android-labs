using System;

namespace NoteList
{
	/// <summary>
	/// Todo Item business object
	/// </summary>
	public class NoteItem
	{
		public NoteItem ()
		{
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime CreationDate { get; set; }
		public byte[] Image { get; set; }
	}
}

