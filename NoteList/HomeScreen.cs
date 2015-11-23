using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace NoteList
{
	[Activity (Label = "NoteList", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		NoteItemListAdapter noteList;
		IList<NoteItem> notes;
		Button addNoteButton;
		ListView noteListView;

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.ActionBarMenu, menu);
			return true;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			noteListView = FindViewById<ListView> (Resource.Id.NoteList);
			addNoteButton = FindViewById<Button> (Resource.Id.AddButton);

			// wire up add note button handler
			if(addNoteButton != null) {
				addNoteButton.Click += (sender, e) => {
					StartActivity(typeof(NoteItemScreen));
				};
			}

			// wire up note click handler
			if(noteListView != null) {
				noteListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var noteDetails = new Intent (this, typeof (NoteItemScreen));
					noteDetails.PutExtra ("NoteID", notes[e.Position].ID);
					StartActivity (noteDetails);
				};
			}
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			notes = NoteItemManager.GetNotes();

			// create our adapter
			noteList = new NoteItemListAdapter(this, notes);

			//Hook up our adapter to our ListView
			noteListView.Adapter = noteList;
		}
	}
}


