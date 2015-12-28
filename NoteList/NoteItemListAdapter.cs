using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.Graphics;
using NoteList.DAL;
using Uri = Android.Net.Uri;

namespace NoteList
{
	/// <summary>
	/// Adapter that presents Notes in a row-view
	/// </summary>
	public class NoteItemListAdapter : BaseAdapter<NoteItem> 
	{
		string currentTheme;
		string currentFontSize;

		readonly Activity context = null;
		IList<NoteItem> notesToDisplay = new List<NoteItem>();
		IList<NoteItem> allNotes = new List<NoteItem>();

		public NoteItemListAdapter (Activity context, IList<NoteItem> notes, string currentTheme, string currentFontSize) : base ()
		{
			this.context = context;
			this.allNotes = notes;
			this.notesToDisplay = allNotes;
			this.currentTheme = currentTheme;
			this.currentFontSize = currentFontSize;
		}

		public override NoteItem this[int position]
		{
			get { return notesToDisplay[position]; }
		}

		public override long GetItemId (int position)
		{
			return notesToDisplay[position].ID;
		}

		public override int Count
		{
			get { return notesToDisplay.Count; }
		}

		public void InvokeFilter(string text)
		{
			notesToDisplay = new List<NoteItem> ();
			foreach (var note in allNotes) {
				if (note.Description.ToLower().Contains (text.ToLower()) || 
					note.Name.ToLower().Contains(text.ToLower())) 
				{
					notesToDisplay.Add (note);
				}
			}
			NotifyDataSetChanged ();
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var item = notesToDisplay[position];

			var view = (convertView ?? 
					context.LayoutInflater.Inflate(
						Resource.Layout.NoteListItem, 
						parent, 
						false)) as LinearLayout;
			
			var txtName = view.FindViewById<TextView>(Resource.Id.NameText);
			var txtDescription = view.FindViewById<TextView>(Resource.Id.DescriptionText);
			var imgPriority = view.FindViewById<ImageView>(Resource.Id.PriorityImage);
			var imgNote = view.FindViewById<ImageView> (Resource.Id.NoteImage);
			var txtDate = view.FindViewById<TextView> (Resource.Id.CreationDate);

			txtName.SetText (item.Name, TextView.BufferType.Normal);
			txtDescription.SetText (item.Description, TextView.BufferType.Normal);

			//var inStream = ContentResolver.OpenInputStream(Uri.Parse("http://xamarin.com/resources/design/home/devices.png"));
			//Android.Graphics.Bitmap bm = Android.Graphics.BitmapFactory.DecodeStream(inStream);
			imgNote.SetImageURI(Uri.Parse(item.ImageUri));

			txtDate.SetText (item.CreationDate.ToString("g"), TextView.BufferType.Normal);
			switch (item.Priority) {
			case 0:
				imgPriority.SetImageResource (Resource.Drawable.Minor);
				break;
			case 1:
				imgPriority.SetImageResource (Resource.Drawable.Major);
				break;
			case 2:
				imgPriority.SetImageResource (Resource.Drawable.Critical);
				break;
			}


			switch (currentTheme) 
			{
			case "0": // Dark
				txtName.SetTextColor (Color.White);
				txtDescription.SetTextColor (Color.White);
				txtDate.SetTextColor (Color.White);
				break;
			case "1": // Light
				txtName.SetTextColor (Color.Blue);
				txtDescription.SetTextColor (Color.Blue);
				txtDate.SetTextColor (Color.Blue);
				break;
			}


			switch (currentFontSize) 
			{
			case "0": // Small
				txtName.SetTextSize (new Android.Util.ComplexUnitType(), 26);
				txtDescription.SetTextSize (new Android.Util.ComplexUnitType(), 16);
				txtDate.SetTextSize (new Android.Util.ComplexUnitType(), 12);
				break;
			case "1": // Medium
				txtName.SetTextSize (new Android.Util.ComplexUnitType(), 32);
				txtDescription.SetTextSize (new Android.Util.ComplexUnitType(), 24);
				txtDate.SetTextSize (new Android.Util.ComplexUnitType(), 16);
				break;
			case "2": // Large
				txtName.SetTextSize (new Android.Util.ComplexUnitType(), 42);
				txtDescription.SetTextSize (new Android.Util.ComplexUnitType(), 32);
				txtDate.SetTextSize (new Android.Util.ComplexUnitType(), 24);
				break;
			}
			return view;
		}
	}
}

