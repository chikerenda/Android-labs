using System.Collections.Generic;
using Android.App;
using Android.Widget;

namespace NoteList
{
	/// <summary>
	/// Adapter that presents Notes in a row-view
	/// </summary>
	public class NoteItemListAdapter : BaseAdapter<NoteItem> 
	{
		Activity context = null;
		IList<NoteItem> notes = new List<NoteItem>();

		public NoteItemListAdapter (Activity context, IList<NoteItem> notes) : base ()
		{
			this.context = context;
			this.notes = notes;
		}

		public override NoteItem this[int position]
		{
			get { return notes[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return notes.Count; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = notes[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()

			//			var view = (convertView ?? 
			//					context.LayoutInflater.Inflate(
			//					Resource.Layout.NoteListItem, 
			//					parent, 
			//					false)) as LinearLayout;
			//			// Find references to each subview in the list item's view
			//			var txtName = view.FindViewById<TextView>(Resource.Id.NameText);
			//			var txtDescription = view.FindViewById<TextView>(Resource.Id.NotesText);
			//			//Assign item's values to the various subviews
			//			txtName.SetText (item.Name, TextView.BufferType.Normal);
			//			txtDescription.SetText (item.Notes, TextView.BufferType.Normal);

			// TODO: use this code to populate the row, and remove the above view
			var view = (convertView ??
				context.LayoutInflater.Inflate(
					Android.Resource.Layout.SimpleListItemChecked,
					parent,
					false)) as CheckedTextView;
			view.SetText (item.Name==""?"<new note>":item.Name, TextView.BufferType.Normal);
			//view.Checked = item.Done;


			//Finally return the view
			return view;
		}
	}
}

