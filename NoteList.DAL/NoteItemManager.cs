using System.Collections.Generic;

namespace NoteList.DAL
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
			return NoteItemRepository.GetNote(id);
		}

		public static List<NoteItem> GetNotes ()
		{
			return new List<NoteItem> (NoteItemRepository.GetNotes());
		}

		public static int SaveNote (NoteItem item)
		{
			return NoteItemRepository.SaveNote(item);
		}

		public static int DeleteNote(int id)
		{
			return NoteItemRepository.DeleteNote(id);
		}
	}
}

