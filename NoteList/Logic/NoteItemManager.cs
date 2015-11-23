using System.Collections.Generic;

namespace NoteList
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class NoteItemManager 
	{
		static NoteItemManager ()
		{
		}

		public static NoteItem GetNote(int id)
		{
			return NoteItemRepositoryADO.GetNote(id);
		}

		public static IList<NoteItem> GetNotes ()
		{
			return new List<NoteItem>(NoteItemRepositoryADO.GetNotes());
		}

		public static int SaveNote (NoteItem item)
		{
			return NoteItemRepositoryADO.SaveNote(item);
		}

		public static int DeleteNote(int id)
		{
			return NoteItemRepositoryADO.DeleteNote(id);
		}
	}
}

